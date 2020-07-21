using Discord.Gateway;
using Discord.Media;
using Discord.Streaming;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discord.Voice
{
    public class DiscordVoiceSession : DiscordMediaSession
    {
        private class SSRCDictionaryEventArgs : EventArgs
        {
            public uint SSRC { get; private set; }
            public ulong UserId { get; private set; }

            public SSRCDictionaryEventArgs(uint ssrc, ulong userId)
            {
                SSRC = ssrc;
                UserId = userId;
            }
        }

        internal object VoiceLock { get; private set; }
        internal ushort Sequence { get; set; }
        internal uint Timestamp { get; set; }

        public bool Speaking { get; private set; }

        public delegate void ConnectHandler(DiscordVoiceSession session, EventArgs e);
        public event ConnectHandler OnConnected;

        public delegate void DisconnectHandler(DiscordVoiceSession session, DiscordMediaCloseEventArgs args);
        public event DisconnectHandler OnDisconnected;

        public delegate void SpeakingStateHandler(DiscordVoiceSession session, DiscordSpeakingStateEventArgs args);
        public event SpeakingStateHandler OnSpeakingStateUpdate;

        public delegate void UserConnectHandler(DiscordVoiceSession session, ulong userId);
        public event UserConnectHandler OnUserConnected;

        public delegate void UserDisconnectHandler(DiscordVoiceSession session, ulong userId);
        public event UserDisconnectHandler OnUserDisconnected;

        private readonly Dictionary<uint, ulong> _ssrcToUserDictionary;
        private readonly Dictionary<ulong, DiscordVoiceReceiver> _receivers;
        private readonly OpusDecoder _decoder;

        internal DiscordVoiceSession(DiscordSocketClient client, DiscordMediaServer server, ulong channelId) : base(client, server, channelId)
        {
            _ssrcToUserDictionary = new Dictionary<uint, ulong>();
            _receivers = new Dictionary<ulong, DiscordVoiceReceiver>();
            _decoder = new OpusDecoder();
            VoiceLock = new object();
        }

        protected override ulong GetServerId()
        {
            return Server.Guild == null ? Channel.Id : Server.Guild.Id;
        }


        public void SetSpeakingState(DiscordVoiceSpeakingState state)
        {
            if (State != DiscordMediaClientState.Connected)
                throw new InvalidOperationException("Connection has been closed.");

            Send(DiscordMediaOpcode.Speaking, new DiscordSpeakingRequest()
            {
                State = state,
                Delay = 0,
                SSRC = SSRC.Audio
            });

            Speaking = state != DiscordVoiceSpeakingState.NotSpeaking;
        }

        public async Task<DiscordLiveStream> StartLivestreamAsync()
        {
            if (Server.Guild == null) // private channel. going for screenshare
            {
                Client.ChangeVoiceState(new VoiceStateChange() { ChannelId = Channel.Id, Screensharing = true });

                return new DiscordLiveStream(this);
            }
            else
                return new DiscordLiveStream(await Client.GoLiveAsync(Server.Guild.Id, Channel.Id));
        }

        public DiscordLiveStream StartLivestream()
        {
            return StartLivestreamAsync().GetAwaiter().GetResult();
        }


        public override void Disconnect()
        {
            try
            {
                Client.ChangeVoiceState(new VoiceStateChange() { GuildId = Client.User.Type == DiscordUserType.User ? null : (ulong?)Server.Guild.Id, ChannelId = null });
            }
            catch { }

            base.Disconnect();
        }


        public DiscordVoiceStream CreateStream(uint bitrate, AudioApplication application = AudioApplication.Mixed)
        {
            if (State != DiscordMediaClientState.Connected)
                throw new InvalidOperationException("Connection has been closed.");

            return new DiscordVoiceStream(this, (int)bitrate, application);
        }


        public DiscordVoiceReceiver CreateReceiver(ulong userId)
        {
            if (_ssrcToUserDictionary.Values.Contains(userId))
            {
                var receiver = new DiscordVoiceReceiver(this, userId);
                _receivers[userId] = receiver;
                return receiver;
            }
            else
                throw new InvalidOperationException("This user has not been registered. Due to how Discord works, this session will not be aware of users that joined the channel before you until they have spoken.");
        }


        protected override void HandlePacket(RTPPacketHeader header, byte[] payload)
        {
            // for some reason discord sends us voice packets before we get the user's ID. i don't think this impacts the audio tho: it seems like these packets don't have any voice data
            if (header.Type == OpusEncoder.Codec.PayloadType && _ssrcToUserDictionary.TryGetValue(header.SSRC, out ulong userId))
            {
                try
                {
                    byte[] decoded = new byte[OpusEncoder.FrameBytes];
                    int length = _decoder.DecodeFrame(payload, 0, payload.Length, decoded, 0, false);

                    if (_receivers.TryGetValue(userId, out DiscordVoiceReceiver receiver))
                        receiver.Enqueue(new DiscordVoicePacket(decoded));
                }
                catch (OpusException) { }
            }
        }


        protected override void HandleResponse(DiscordMediaResponse response)
        {
            switch (response.Opcode)
            {
                case DiscordMediaOpcode.Speaking:
                    var state = response.Deserialize<DiscordSpeakingStateEventArgs>();

                    if (state.UserId == 0)
                        state.UserId = _ssrcToUserDictionary[state.SSRC];
                    else
                        _ssrcToUserDictionary[state.SSRC] = state.UserId;

                    OnSpeakingStateUpdate?.Invoke(this, state);
                    break;
                case DiscordMediaOpcode.SSRCUpdate: // this is fired whenever a user connects to the channel or updates their ssrc
                    DiscordSSRC ssrc = response.Deserialize<DiscordSSRC>();

                    bool newUser = !_ssrcToUserDictionary.Values.Contains(ssrc.UserId);

                    _ssrcToUserDictionary[ssrc.Audio] = ssrc.UserId;

                    if (newUser)
                        OnUserConnected?.Invoke(this, ssrc.UserId);
                    break;
                case DiscordMediaOpcode.UserDisconnect:
                    ulong userId = response.Deserialize<JObject>().Value<ulong>("user_id");

                    foreach (var item in new List<KeyValuePair<uint, ulong>>(_ssrcToUserDictionary.Where(i => i.Value == userId)))
                        _ssrcToUserDictionary.Remove(item.Key);

                    if (_receivers.TryGetValue(userId, out DiscordVoiceReceiver receiver))
                    {
                        receiver.Close();
                        _receivers.Remove(receiver.UserId);
                    }

                    OnUserDisconnected?.Invoke(this, userId);
                    break;
            }
        }

        protected override void HandleConnect()
        {
            OnConnected?.Invoke(this, new EventArgs());
        }

        protected override void HandleDisconnect(DiscordMediaCloseEventArgs args)
        {
            OnDisconnected?.Invoke(this, args);
        }
    }
}

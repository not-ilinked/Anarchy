using Anarchy;
using Discord.Gateway;
using Discord.WebSockets;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Discord.Media
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
        internal static readonly byte[] SilenceFrame = new byte[] { 0xF8, 0xFF, 0xFE };

        public bool Speaking { get; private set; }
        public DiscordGoLiveSession Livestream { get; internal set; }

        internal Dictionary<string, DiscordGoLiveSession> WatchingDictionary { get; private set; }
        public IReadOnlyList<DiscordGoLiveSession> Watching
        {
            get { return WatchingDictionary.Values.ToList(); }
        }

        public delegate void ConnectHandler(DiscordVoiceSession session, EventArgs e);
        public event ConnectHandler OnConnected;

        public delegate void DisconnectHandler(DiscordVoiceSession session, DiscordMediaCloseEventArgs args);
        public event DisconnectHandler OnDisconnected;

        public delegate void UserConnectHandler(DiscordVoiceSession session, ulong userId);
        public event UserConnectHandler OnUserConnected;

        public delegate void SpeakingHandler(DiscordVoiceSession session, IncomingVoiceStream stream);
        public event SpeakingHandler OnUserSpeaking;

        public delegate void UserDisconnectHandler(DiscordVoiceSession session, ulong userId);
        public event UserDisconnectHandler OnUserDisconnected;

        public delegate void ChannelHandler(DiscordVoiceSession session, ChannelChangedEventArgs args);
        public event ChannelHandler OnChannelChanged;

        internal new ulong ChannelId
        {
            get { return base.ChannelId; }
            set
            {
                ulong old = ChannelId;
                base.ChannelId = value;

                if (OnChannelChanged != null)
                    Task.Run(() => OnChannelChanged?.Invoke(this, new ChannelChangedEventArgs(old, value)));
            }
        }

        private ConcurrentDictionary<uint, ulong> _ssrcToUserDictionary;
        private ConcurrentDictionary<ulong, IncomingVoiceStream> _receivers;
        private OpusDecoder _decoder;
        private bool _sodiumExists;

        private void Initialize()
        {
            try
            {
                _decoder = new OpusDecoder();
            }
            catch (DllNotFoundException) { }

            _sodiumExists =  File.Exists("libsodium.dll");

            _ssrcToUserDictionary = new ConcurrentDictionary<uint, ulong>();
            _receivers = new ConcurrentDictionary<ulong, IncomingVoiceStream>();
            VoiceLock = new object();
            WatchingDictionary = new Dictionary<string, DiscordGoLiveSession>();
        }

        internal DiscordVoiceSession(DiscordSocketClient client, ulong? guildId, ulong channelId, string sessionId) : base(client, guildId, channelId, sessionId)
        {
            Initialize();
        }

        internal DiscordVoiceSession(DiscordVoiceSession other) : base(other)
        {
            other.OnConnected = null;
            other.OnDisconnected = null;
            other.OnUserConnected = null;
            other.OnUserSpeaking = null;
            other.OnUserDisconnected = null;
            other.OnChannelChanged = null;

            Initialize();
        }

        protected override ulong GetServerId()
        {
            return CurrentServer.Guild == null ? Channel.Id : CurrentServer.Guild.Id;
        }


        public void SetChannel(ulong channelId)
        {
            Client.ChangeVoiceState(new VoiceStateProperties() { GuildId = GuildId, ChannelId = channelId });
        }


        public void SetSpeakingState(DiscordSpeakingFlags flags)
        {
            if (State != MediaSessionState.Authenticated)
                throw new InvalidOperationException("Connection has been closed.");

            Send(DiscordMediaOpcode.Speaking, new DiscordSpeakingRequest()
            {
                State = flags,
                Delay = 0,
                SSRC = SSRC.Audio
            });

            Speaking = flags != DiscordSpeakingFlags.NotSpeaking;
        }


        public Task<DiscordGoLiveSession> GoLiveAsync()
        {
            return JoinGoLiveAsync(Guild.Id, Channel.Id, () => Client.Send(GatewayOpcode.GoLive, new StartStream() 
            { 
                Type = "guild", 
                GuildId = Guild.Id, 
                ChannelId = Channel.Id 
            }));
        }

        public DiscordGoLiveSession GoLive()
        {
            return GoLiveAsync().GetAwaiter().GetResult();
        }


        public Task<DiscordGoLiveSession> WatchGoLiveAsync(ulong userId)
        {
            return JoinGoLiveAsync(Guild.Id, Channel.Id, () => Client.Send(GatewayOpcode.WatchGoLive, new GoLiveStreamKey() 
            { 
                StreamKey = new StreamKey()
                { 
                    GuildId = Guild.Id, 
                    ChannelId = Channel.Id, 
                    UserId = userId 
                }.Serialize()
            }));
        }

        public DiscordGoLiveSession WatchGoLive(ulong userId)
        {
            return WatchGoLiveAsync(userId).GetAwaiter().GetResult();
        }
    

        public override void Disconnect()
        {
            try
            {
                // discord rlly wants us to do this first for some reason
                Client.ChangeVoiceState(new VoiceStateProperties() { GuildId = Guild.Id, ChannelId = null });
            }
            catch (InvalidOperationException) { }

            base.Disconnect();
        }


        public DiscordVoiceStream CreateStream(uint bitrate, AudioApplication application = AudioApplication.Mixed)
        {
            if (State != MediaSessionState.Authenticated)
                throw new InvalidOperationException("Connection has been closed.");

            return new DiscordVoiceStream(this, (int)bitrate, application);
        }


        protected override void HandlePacket(RTPPacketHeader header, byte[] payload)
        {
            // for some reason discord sends us voice packets before we get the user's ID. i don't think this impacts the audio tho; it seems like these packets don't have any voice data
            if (_decoder != null && _sodiumExists && header.Type == SupportedCodecs["opus"].PayloadType && _ssrcToUserDictionary.TryGetValue(header.SSRC, out ulong userId))
            {
                if (!_receivers.TryGetValue(userId, out IncomingVoiceStream receiver))
                {
                    receiver = _receivers[userId] = new IncomingVoiceStream(this, userId);

                    if (OnUserSpeaking != null)
                        Task.Run(() => OnUserSpeaking.Invoke(this, _receivers[userId]));
                }

                if (payload.SequenceEqual(SilenceFrame))
                {
                    receiver.SilenceFramesReceived++;

                    if (receiver.SilenceFramesReceived >= 10)
                    {
                        receiver.Close();
                        _receivers.Remove(receiver.UserId);
                    }
                }
                else
                {
                    try
                    {
                        byte[] decoded = new byte[OpusEncoder.FrameBytes];
                        int length = _decoder.DecodeFrame(payload, 0, payload.Length, decoded, 0, false);

                        receiver.Enqueue(new DiscordVoicePacket(decoded));
                    }
                    catch (OpusException) { }
                }
            }
        }


        protected override void HandleMessage(DiscordWebSocketMessage<DiscordMediaOpcode> message)
        {
            switch (message.Opcode)
            {
                case DiscordMediaOpcode.Speaking:
                    var state = message.Data.ToObject<DiscordSpeakingState>();

                    if (state.UserId.HasValue)
                        _ssrcToUserDictionary[state.SSRC] = state.UserId.Value;
                    else
                        state.UserId = _ssrcToUserDictionary[state.SSRC];

                    break;
                case DiscordMediaOpcode.SSRCUpdate: // this is fired whenever a user connects to the channel or updates their ssrc
                    SSRCUpdate update = message.Data.ToObject<SSRCUpdate>();

                    bool newUser = !_ssrcToUserDictionary.Values.Contains(update.UserId);

                    _ssrcToUserDictionary[update.Audio] = update.UserId;

                    if (newUser && OnUserConnected != null)
                        Task.Run(() => OnUserConnected.Invoke(this, update.UserId));
                    break;
                case DiscordMediaOpcode.UserDisconnect:
                    ulong userId = message.Data.ToObject<JObject>().Value<ulong>("user_id");

                    if (_ssrcToUserDictionary.TryGetKey(userId, out uint ssrc))
                        _ssrcToUserDictionary.Remove(ssrc);

                    if (_receivers.TryGetValue(userId, out IncomingVoiceStream receiver))
                    {
                        receiver.Close();
                        _receivers.Remove(receiver.UserId);
                    }

                    if (OnUserDisconnected != null)
                        Task.Run(() => OnUserDisconnected.Invoke(this, userId));
                    break;
            }
        }

        protected override void HandleConnect()
        {
            try
            {
                SetSSRC(SSRC.Audio);

                OnConnected?.Invoke(this, new EventArgs());
            }
            catch (InvalidOperationException) { }
        }

        protected override void HandleDisconnect(DiscordMediaCloseEventArgs args)
        {
            if (args.Code != DiscordMediaCloseCode.Disconnected) // Disconnected is used when switching channel in bigger guilds
                Task.Run(() => OnDisconnected?.Invoke(this, args));
        }

        internal void Kill()
        {
            State = MediaSessionState.Dead;
            Task.Run(() => OnDisconnected?.Invoke(this, new DiscordMediaCloseEventArgs(DiscordMediaCloseCode.Disconnected, "Disconnected.")));
        }

        private Task<DiscordGoLiveSession> JoinGoLiveAsync(ulong guildId, ulong channelId, Action caller)
        {
            TaskCompletionSource<DiscordGoLiveSession> task = new TaskCompletionSource<DiscordGoLiveSession>();

            void serverHandler(DiscordMediaSession session, DiscordMediaServer server)
            {
                session.OnServerUpdated -= serverHandler;
                task.SetResult((DiscordGoLiveSession)session);
            }

            void createHandler(DiscordSocketClient c, GoLiveCreate stream)
            {
                StreamKey key = StreamKey.Deserialize(stream.StreamKey);

                if (key.GuildId == guildId && key.ChannelId == channelId)
                {
                    Client.OnStreamCreated -= createHandler;

                    var session = new DiscordGoLiveSession(this, guildId, channelId, stream);
                    session.OnServerUpdated += serverHandler;
                    Client.Livestreams[stream.StreamKey] = session;
                }
            }

            void deleteHandler(DiscordSocketClient c, GoLiveDelete delete)
            {
                StreamKey key = StreamKey.Deserialize(delete.StreamKey);

                if (key.GuildId == guildId && key.ChannelId == channelId)
                {
                    Client.OnStreamDeleted -= deleteHandler;

                    task.SetException(new DiscordGoLiveException(delete));
                }
            }

            Client.OnStreamCreated += createHandler;
            Client.OnStreamDeleted += deleteHandler;

            caller();

            return task.Task;
        }
    }
}

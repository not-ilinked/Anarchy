using Discord.Gateway;
using Discord.WebSockets;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Discord.Media
{
    public class DiscordGoLiveSession : DiscordMediaSession, IDisposable
    {
        private readonly ulong _rtcServerId;
        public string StreamKey { get; private set; }
        public IReadOnlyList<ulong> Viewers { get; private set; }
        public bool Paused { get; private set; }
        public DiscordVoiceSession ParentSession { get; private set; }

        public delegate void ConnectHandler(DiscordGoLiveSession session, EventArgs e);
        public event ConnectHandler OnConnected;

        public delegate void DisconnectHandler(DiscordGoLiveSession session, DiscordGoLiveCloseEventArgs args);
        public event DisconnectHandler OnDisconnected;

        public delegate void UserConnectHandler(DiscordGoLiveSession session, ulong userId);
        public event UserConnectHandler OnUserConnected;

        public delegate void UserDisconnectHandler(DiscordGoLiveSession session, ulong userId);
        public event UserDisconnectHandler OnUserDisconnected;


        internal DiscordGoLiveSession(DiscordVoiceSession parentSession, ulong? guildId, ulong channelId, GoLiveCreate goLive) : base(parentSession.Client, guildId, channelId, goLive.SessionId)
        {
            ReceivePackets = false; // we don't have any parsers yet so we might as well not get the packets
            _rtcServerId = goLive.RtcServerId;
            StreamKey = goLive.StreamKey;
            Update(goLive);
            ParentSession = parentSession;
        }


        internal void Update(GoLiveUpdate update)
        {
            Paused = update.Paused;
            Viewers = update.ViewerIds.ToList();
        }


        protected override ulong GetServerId()
        {
            return _rtcServerId;
        }


        public override void Disconnect()
        {
            Client.EndGoLive(CurrentServer.StreamKey);

            base.Disconnect();
        }

        internal void Disconnect(GoLiveDelete delete)
        {
            base.Disconnect((ushort)UnorthodoxCloseCode.KeepQuiet, "Disconnecting with Go Live error");
            OnDisconnected?.Invoke(this, new DiscordGoLiveCloseEventArgs(DiscordMediaCloseCode.Disconnected, "Disconnected from livestream: " + delete.Reason, delete));
        }


        protected override void HandleConnect()
        {
            if (StreamKey.EndsWith(Client.User.Id.ToString()))
            {
                Client.Send(GatewayOpcode.GoLiveUpdate, new StreamUpdate() { StreamKey = StreamKey, Paused = false });

                SetSSRC(SSRC.Audio);
            }

            OnConnected?.Invoke(this, new EventArgs());
        }


        protected override void HandleDisconnect(DiscordMediaCloseEventArgs args)
        {
            State = MediaSessionState.Dead;
            OnDisconnected?.Invoke(this, new DiscordGoLiveCloseEventArgs(args.Code, args.Reason));
        }


        protected override void HandleMessage(DiscordWebSocketMessage<DiscordMediaOpcode> message)
        {
            if (message.Opcode == DiscordMediaOpcode.SSRCUpdate)
            {
                SSRCUpdate ssrc = message.Data.ToObject<SSRCUpdate>();

                if (!Viewers.Contains(ssrc.UserId))
                {
                    List<ulong> viewers = Viewers.ToList();
                    viewers.Add(ssrc.UserId);
                    Viewers = viewers;
                    OnUserConnected?.Invoke(this, ssrc.UserId);
                }
            }
            else if (message.Opcode == DiscordMediaOpcode.UserDisconnect)
            {
                ulong userId = message.Data.ToObject<JObject>().Value<ulong>("user_id");

                List<ulong> viewers = Viewers.ToList();
                if (viewers.Remove(userId))
                    Viewers = viewers;

                OnUserDisconnected?.Invoke(this, userId);
            }
        }

        public new void Dispose()
        {
            StreamKey = null;
            Viewers = null;
            base.Dispose();
        }
    }
}

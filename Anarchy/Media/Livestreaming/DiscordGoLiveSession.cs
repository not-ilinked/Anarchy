using Discord.Gateway;
using Discord.Media;
using System;

namespace Discord.Streaming
{
    internal class DiscordGoLiveSession : DiscordMediaSession
    {
        private readonly GoLiveCreate _goLive;

        public delegate void ConnectHandler(DiscordGoLiveSession session, EventArgs e);
        public event ConnectHandler OnConnected;

        public delegate void DisconnectHandler(DiscordGoLiveSession session, DiscordMediaCloseEventArgs args);
        public event DisconnectHandler OnDisconnected;

        internal DiscordGoLiveSession(DiscordSocketClient client, DiscordMediaServer server, ulong channelId, GoLiveCreate goLive) : base(client, server, channelId)
        {
            _goLive = goLive;
        }


        protected override ulong GetServerId()
        {
            return _goLive.RtcServerId;
        }


        public override void Disconnect()
        {
            Client.EndGoLive(Server.StreamKey);

            base.Disconnect();
        }


        protected override void HandleConnect()
        {
            Client.Send(GatewayOpcode.GoLiveUpdate, new StreamUpdate() { StreamKey = _goLive.StreamKey, Paused = false });

            OnConnected?.Invoke(this, new EventArgs());
        }


        protected override void HandleDisconnect(DiscordMediaCloseEventArgs args)
        {
            OnDisconnected?.Invoke(this, args);
        }
    }
}

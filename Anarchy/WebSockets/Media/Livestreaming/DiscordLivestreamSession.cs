using System.Collections.Generic;
using System.Linq;
using Discord.Gateway;
using Newtonsoft.Json.Linq;

namespace Discord.Media
{
    public class DiscordLivestreamSession
    {
        public delegate void SessionHandler(DiscordLivestreamSession session);
        internal event SessionHandler OnConnected;

        public delegate void UserConnectHandler(DiscordLivestreamSession session, ulong userId);
        public event UserConnectHandler OnUserConnected;

        public delegate void UserDisconnectHandler(DiscordLivestreamSession session, ulong userId);
        public event UserDisconnectHandler OnUserDisconnected;


        private readonly StreamKey _streamKey;
        private readonly ulong _rtcServerId;
        private readonly string _sessionId;
        private DiscordMediaConnection _connection;

        public MediaConnectionState State => _connection == null ? MediaConnectionState.NotConnected : _connection.State;

        public MinimalGuild Guild => new MinimalGuild(_streamKey.GuildId).SetClient(Client);
        public MinimalChannel Channel => new MinimalChannel(_streamKey.ChannelId).SetClient(Client);
        public DiscordSocketClient Client { get; }

        public ulong StreamerId => _streamKey.UserId;
        public string StreamKey => _streamKey.Serialize();
        public IReadOnlyList<ulong> Viewers { get; private set; }
        public bool Paused { get; private set; }

        internal DiscordLivestreamSession(DiscordSocketClient client, StreamKey key, ulong rtcServerId, string sessionId)
        {
            Client = client;
            _streamKey = key;
            _rtcServerId = rtcServerId;
            _sessionId = sessionId;
        }

        internal void Update(GoLiveUpdate update)
        {
            Viewers = update.ViewerIds;
            Paused = update.Paused;
        }

        internal void UpdateServer(DiscordMediaServer server)
        {
            _connection = new DiscordMediaConnection(Client, _rtcServerId, server);

            _connection.OnReady += (c) =>
            {
                if (StreamerId == Client.User.Id)
                    Client.Send(GatewayOpcode.GoLiveUpdate, new StreamUpdate() { StreamKey = StreamKey, Paused = false });

                OnConnected?.Invoke(this);
            };

            _connection.OnMessage += HandleMessage;

            _connection.ConnectAsync().GetAwaiter().GetResult();
        }

        public void Disconnect()
        {
            Client.EndGoLive(_streamKey.Serialize());
            _connection.DisconnectAsync(1000, "Closed by client").GetAwaiter().GetResult();
        }

        private void HandleMessage(DiscordMediaConnection connection, WebSockets.DiscordWebSocketMessage<DiscordMediaOpcode> message)
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
    }
}

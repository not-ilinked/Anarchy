using Discord.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Media
{
    public class DiscordLivestreamClient
    {
        public delegate void CreateHandler(DiscordLivestreamClient client, DiscordLivestreamSession session);

        public event CreateHandler OnStartedLivestream;
        public event CreateHandler OnJoinedLivestream;

        public delegate void CloseHandler(DiscordLivestreamClient client, LivestreamDisconnectEventArgs args);
        public event CloseHandler OnLeftLivestream;

        private readonly ulong _guildId;
        public MinimalGuild Guild => new MinimalGuild(_guildId).SetClient(Client);

        private readonly ulong _channelId;
        public MinimalChannel Channel => new MinimalChannel(_channelId).SetClient(Client);

        public DiscordSocketClient Client { get; }

        public DiscordLivestreamClient(DiscordSocketClient client, ulong guildId, ulong channelId)
        {
            _watching = new Dictionary<ulong, DiscordLivestreamSession>();
            Client = client;
            _guildId = guildId;
            _channelId = channelId;
        }

        public DiscordLivestreamSession Own { get; private set; }

        private readonly Dictionary<ulong, DiscordLivestreamSession> _watching;
        public IReadOnlyList<DiscordLivestreamSession> Watching => _watching.Values.ToList();

        internal void CreateSession(GoLiveCreate goLive)
        {
            Console.WriteLine("Creating session");

            var key = new StreamKey(goLive.StreamKey);

            DiscordLivestreamSession session;

            if (key.UserId == Client.User.Id)
            {
                session = Own = new DiscordLivestreamSession(Client, key, goLive.RtcServerId, goLive.SessionId);
                session.OnConnected += s =>
                {
                    if (OnStartedLivestream != null)
                        Task.Run(() => OnStartedLivestream.Invoke(this, s));
                };
            }
            else
            {
                session = _watching[key.UserId] = new DiscordLivestreamSession(Client, key, goLive.RtcServerId, goLive.SessionId);
                session.OnConnected += s =>
                {
                    if (OnJoinedLivestream != null)
                        Task.Run(() => OnJoinedLivestream.Invoke(this, s));
                };
            }

            session.Update(goLive);
        }

        internal void UpdateSession(GoLiveUpdate goLive)
        {
            Console.WriteLine("Updating session");

            var key = new StreamKey(goLive.StreamKey);

            if (key.UserId == Client.User.Id) Own.Update(goLive);
            else _watching[key.UserId].Update(goLive);
        }

        internal void SetSessionServer(ulong userId, DiscordMediaServer server)
        {
            Console.WriteLine("Updating server");

            if (userId == Client.User.Id) Own.UpdateServer(server);
            else _watching[userId].UpdateServer(server);
        }

        internal void KillSession(GoLiveDelete goLive)
        {
            Console.WriteLine("Killing session");

            var key = new StreamKey(goLive.StreamKey);

            if (OnLeftLivestream != null)
                Task.Run(() => OnLeftLivestream.Invoke(this, new LivestreamDisconnectEventArgs(key.UserId, goLive)));
        }

        public void Start()
        {
            Client.Send(GatewayOpcode.GoLive, new StartStream()
            {
                Type = "guild",
                GuildId = _guildId,
                ChannelId = _channelId
            });
        }

        public void Join(ulong userId)
        {
            Client.Send(GatewayOpcode.WatchGoLive, new GoLiveStreamKey()
            {
                StreamKey = new StreamKey(_guildId, _channelId, userId).Serialize()
            });
        }
    }
}

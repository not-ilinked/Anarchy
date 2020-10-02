using Anarchy;
using Discord.Media;

namespace Discord.Gateway
{
    internal class DiscordVoiceSessionManager
    {
        private readonly DiscordSocketClient _client;

        private readonly ConcurrentList<DiscordVoiceSession> _sessions;

        public DiscordVoiceSessionManager(DiscordSocketClient client)
        {
            _client = client;
            _sessions = new ConcurrentList<DiscordVoiceSession>();
        }

        public bool TryGetSession(ulong guildId, out DiscordVoiceSession session)
        {
            lock (_sessions.Lock)
            {
                foreach (var s in _sessions)
                {
                    if (s.Guild == null ? guildId == 0 : guildId == s.Guild.Id)
                    {
                        session = s;
                        return true;
                    }
                }
            }

            session = null;
            return false;
        }

        public void AllowNewConnection(ulong? guildId)
        {
            foreach (var session in _sessions.CreateCopy())
            {
                if (_client.User.Type == DiscordUserType.User || (guildId.HasValue && session.Guild.Id == guildId.Value))
                    session.Disconnect();
            }
        }

        public void DisconnectAll()
        {
            foreach (var session in _sessions.CreateCopy())
            {
                if (session.State == MediaSessionState.Connected)
                    session.Disconnect();
            }
        }

        public DiscordVoiceSession CreateSession(ulong? guildId, ulong channelId)
        {
            DiscordVoiceSession session = new DiscordVoiceSession(_client, guildId, channelId);
            session.OnDisconnected += Session_OnDisconnected;
            _sessions.Add(session);
            return session;
        }

        private void Session_OnDisconnected(DiscordVoiceSession session, DiscordMediaCloseEventArgs args)
        {
            _sessions.Remove(session);
        }
    }
}

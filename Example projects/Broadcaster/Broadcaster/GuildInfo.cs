using Discord;

namespace Broadcaster
{
    public class GuildInfo
    {
        public GuildInfo(string invite)
        {
            Id = new DiscordClient(new DiscordConfig() { GetFingerprint = false }).GetInvite(invite).ToGuildInvite().Guild.Id;
            Invite = invite;
        }

        public string Invite { get; private set; }
        public ulong Id { get; private set; }
    }
}

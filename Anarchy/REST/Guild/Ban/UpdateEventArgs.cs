namespace Discord.Gateway
{
    public class BanUpdateEventArgs
    {
        public ulong GuildId { get; private set; }
        public DiscordUser User { get; private set; }


        internal BanUpdateEventArgs(BanContainer ban)
        {
            GuildId = ban.GuildId;
            User = ban.User;
        }


        public override string ToString()
        {
            return User.ToString();
        }
    }
}

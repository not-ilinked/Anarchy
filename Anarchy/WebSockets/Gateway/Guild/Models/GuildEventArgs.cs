namespace Discord.Gateway
{
    public class GuildEventArgs
    {
        public DiscordGuild Guild { get; private set; }

        internal GuildEventArgs(DiscordGuild guild)
        {
            Guild = guild;
        }

        public override string ToString()
        {
            return Guild.ToString();
        }
    }
}

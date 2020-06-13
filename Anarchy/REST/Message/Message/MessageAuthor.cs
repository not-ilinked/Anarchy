namespace Discord
{
    public class MessageAuthor : Controllable
    {
        public MessageAuthor(DiscordUser user, GuildMember member)
        {
            User = user;

            Member = member;
        }

        public DiscordUser User { get; private set; }
        public GuildMember Member { get; private set; }


        public override string ToString()
        {
            return User.ToString();
        }
    }
}

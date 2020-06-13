namespace Discord
{
    public class UserEventArgs
    {
        public DiscordUser User { get; private set; }

        internal UserEventArgs(DiscordUser user)
        {
            User = user;
        }


        public override string ToString()
        {
            return User.ToString();
        }
    }
}

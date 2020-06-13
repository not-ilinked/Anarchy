namespace Discord
{
    public class ProfileUpdatedEventArgs
    {
        public DiscordProfile Profile { get; private set; }


        internal ProfileUpdatedEventArgs(DiscordProfile profile)
        {
            Profile = profile;
        }
    }
}

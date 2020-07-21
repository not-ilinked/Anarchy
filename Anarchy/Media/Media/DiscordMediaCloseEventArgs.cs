namespace Discord.Media
{
    public class DiscordMediaCloseEventArgs
    {
        public bool HasError { get; private set; }
        public DiscordMediaCloseError Error { get; private set; }

        internal DiscordMediaCloseEventArgs() { }

        internal DiscordMediaCloseEventArgs(DiscordMediaCloseError error)
        {
            HasError = true;
            Error = error;
        }
    }
}

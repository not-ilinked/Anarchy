namespace Discord
{
    public class DiscordSettingsEventArgs
    {
        public DiscordUserSettings Settings { get; private set; }

        public DiscordSettingsEventArgs(DiscordUserSettings update)
        {
            Settings = update;
        }
    }
}

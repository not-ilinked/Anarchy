namespace Discord
{
    public class DiscordWebhookProfile
    {
        internal DiscordParameter<string> NameProperty = new DiscordParameter<string>();
        public string Username
        {
            get => NameProperty;
            set => NameProperty.Value = value;
        }


        internal DiscordParameter<string> AvatarProperty = new DiscordParameter<string>();
        public string AvatarUrl
        {
            get => AvatarProperty;
            set => AvatarProperty.Value = value;
        }
    }
}

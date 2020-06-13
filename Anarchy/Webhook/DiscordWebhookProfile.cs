namespace Discord.Webhook
{
    public class DiscordWebhookProfile
    {
        internal Property<string> NameProperty = new Property<string>();
        public string Username
        {
            get { return NameProperty; }
            set { NameProperty.Value = value; }
        }


        internal Property<string> AvatarProperty = new Property<string>();
        public string AvatarUrl
        {
            get { return AvatarProperty; }
            set { AvatarProperty.Value = value; }
        }
    }
}

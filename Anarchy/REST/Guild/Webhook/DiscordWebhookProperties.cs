using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Options for creating/modifying a webhook
    /// </summary>
    public class DiscordWebhookProperties
    {
        private readonly DiscordParameter<string> NameProperty = new DiscordParameter<string>();
        [JsonProperty("name")]
        public string Name
        {
            get { return NameProperty; }
            set { NameProperty.Value = value; }
        }

        public bool ShouldSerializeName()
        {
            return NameProperty.Set;
        }

        private readonly DiscordParameter<DiscordImage> AvatarProperty = new DiscordParameter<DiscordImage>();
        [JsonProperty("avatar")]
        public DiscordImage Avatar
        {
            get { return AvatarProperty; }
            set { AvatarProperty.Value = value; }
        }

        public bool ShouldSeriaizeAvatar()
        {
            return AvatarProperty.Set;
        }

        private readonly DiscordParameter<ulong> ChannelProperty = new DiscordParameter<ulong>();
        [JsonProperty("channel_id")]
        public ulong ChannelId
        {
            get { return ChannelProperty; }
            set { ChannelProperty.Value = value; }
        }

        public bool ShouldSerializeChannelId()
        {
            return ChannelProperty.Set;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

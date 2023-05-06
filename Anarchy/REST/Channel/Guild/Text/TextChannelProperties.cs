using System.Text.Json.Serialization;

namespace Discord
{
    /// <summary>
    /// Options for modifying a <see cref="TextChannel"/>
    /// </summary>
    public class TextChannelProperties : GuildChannelProperties
    {
        internal readonly DiscordParameter<ChannelType> TypeProperty = new DiscordParameter<ChannelType>();
        [JsonPropertyName("type")]
        private ChannelType _type
        {
            get { return TypeProperty; }
        }

        public bool News
        {
            get { return TypeProperty == ChannelType.News; }
            set { TypeProperty.Value = value ? ChannelType.News : ChannelType.Text; }
        }

        public bool ShouldSerialize_type()
        {
            return TypeProperty.Set;
        }

        private readonly DiscordParameter<string> TopicProperty = new DiscordParameter<string>();
        [JsonPropertyName("topic")]
        public string Topic
        {
            get { return TopicProperty; }
            set { TopicProperty.Value = value; }
        }

        public bool ShouldSerializeTopic()
        {
            return TopicProperty.Set;
        }

        private readonly DiscordParameter<bool> NsfwProperty = new DiscordParameter<bool>();
        [JsonPropertyName("nsfw")]
        public bool Nsfw
        {
            get { return NsfwProperty; }
            set { NsfwProperty.Value = value; }
        }

        public bool ShouldSerializeNsfw()
        {
            return NsfwProperty.Set;
        }

        private readonly DiscordParameter<int> SlowModeProperty = new DiscordParameter<int>();
        [JsonPropertyName("rate_limit_per_user")]
        public int SlowMode
        {
            get { return SlowModeProperty; }
            set { SlowModeProperty.Value = value; }
        }

        public bool ShouldSerializeSlowMode()
        {
            return SlowModeProperty.Set;
        }
    }
}

using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Options for modifying a <see cref="TextChannel"/>
    /// </summary>
    public class TextChannelProperties : GuildChannelProperties
    {
        private readonly Property<string> TopicProperty = new Property<string>();
        [JsonProperty("topic")]
        public string Topic
        {
            get { return TopicProperty; }
            set { TopicProperty.Value = value; }
        }


        public bool ShouldSerializeTopic()
        {
            return TopicProperty.Set;
        }


        private readonly Property<bool> NsfwProperty = new Property<bool>();
        [JsonProperty("nsfw")]
        public bool Nsfw
        {
            get { return NsfwProperty; }
            set { NsfwProperty.Value = value; }
        }


        public bool ShouldSerializeNsfw()
        {
            return NsfwProperty.Set;
        }


        private readonly Property<int> SlowModeProperty = new Property<int>();
        [JsonProperty("rate_limit_per_user")]
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

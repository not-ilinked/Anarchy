using System.Text.Json.Serialization;

namespace Discord
{
    /// <summary>
    /// Options for modifying a <see cref="DiscordGroup"/>
    /// </summary>
    public class GroupProperties
    {
        private readonly DiscordParameter<string> NameProperty = new DiscordParameter<string>();
        [JsonPropertyName("name")]
        public string Name
        {
            get { return NameProperty; }
            set { NameProperty.Value = value; }
        }

        public bool ShouldSerializeName()
        {
            return NameProperty.Set;
        }

        internal readonly DiscordParameter<DiscordImage> IconProperty = new DiscordParameter<DiscordImage>();
        [JsonPropertyName("icon")]
        public DiscordImage Icon
        {
            get { return IconProperty; }
            set { IconProperty.Value = value; }
        }

        public bool ShouldSerializeIcon()
        {
            return IconProperty.Set;
        }
    }
}

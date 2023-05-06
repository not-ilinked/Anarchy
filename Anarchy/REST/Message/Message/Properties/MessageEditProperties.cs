

using System.Text.Json.Serialization;

namespace Discord
{
    public class MessageEditProperties
    {
        private readonly DiscordParameter<string> _contentProperty = new();
        [JsonPropertyName("content")]
        public string Content
        {
            get { return _contentProperty; }
            set { _contentProperty.Value = value; }
        }

        public bool ShouldSerializeContent()
        {
            return _contentProperty.Set;
        }

        private readonly DiscordParameter<DiscordEmbed> _embedProperty = new();
        [JsonPropertyName("embed")]
        public DiscordEmbed Embed
        {
            get { return _embedProperty; }
            set { _embedProperty.Value = value; }
        }

        public bool ShouldSerializeEmbed()
        {
            return _embedProperty.Set;
        }

        private readonly DiscordParameter<MessageFlags> _flagProperty = new();
        [JsonPropertyName("flags")]
        public MessageFlags Flags
        {
            get { return _flagProperty; }
            set { _flagProperty.Value = value; }
        }

        public bool ShouldSerializeFlags()
        {
            return _flagProperty.Set;
        }
    }
}

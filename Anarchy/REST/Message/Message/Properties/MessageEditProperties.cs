using Newtonsoft.Json;

namespace Discord
{
    public class MessageEditProperties
    {
        private readonly DiscordParameter<string> _contentProperty = new DiscordParameter<string>();
        [JsonProperty("content")]
        public string Content
        {
            get => _contentProperty;
            set => _contentProperty.Value = value;
        }

        public bool ShouldSerializeContent()
        {
            return _contentProperty.Set;
        }


        private readonly DiscordParameter<DiscordEmbed> _embedProperty = new DiscordParameter<DiscordEmbed>();
        [JsonProperty("embed")]
        public DiscordEmbed Embed
        {
            get => _embedProperty;
            set => _embedProperty.Value = value;
        }

        public bool ShouldSerializeEmbed()
        {
            return _embedProperty.Set;
        }


        private readonly DiscordParameter<MessageFlags> _flagProperty = new DiscordParameter<MessageFlags>();
        [JsonProperty("flags")]
        public MessageFlags Flags
        {
            get => _flagProperty;
            set => _flagProperty.Value = value;
        }

        public bool ShouldSerializeFlags()
        {
            return _flagProperty.Set;
        }
    }
}

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord
{
    public class InteractionResponseProperties
    {
        private readonly DiscordParameter<bool> _ttsParam = new DiscordParameter<bool>();
        [JsonPropertyName("tts")]
        public bool Tts
        {
            get { return _ttsParam; }
            set { _ttsParam.Value = value; }
        }

        public bool ShouldSerializeTts() => _ttsParam.Set;

        private readonly DiscordParameter<string> _contentParam = new DiscordParameter<string>();
        [JsonPropertyName("content")]
        public string Content
        {
            get { return _contentParam; }
            set { _contentParam.Value = value; }
        }

        public bool ShouldSerializeContent() => _contentParam.Set;

        private readonly DiscordParameter<List<DiscordEmbed>> _embedParam = new DiscordParameter<List<DiscordEmbed>>();
        [JsonPropertyName("embeds")]
        private List<DiscordEmbed> _embeds => _embedParam.Value;

        [JsonIgnore]
        public DiscordEmbed Embed
        {
            get
            {
                return _embedParam.Value?[0];
            }
            set
            {
                if (value == null) _embedParam.Value = null;
                else _embedParam.Value = new List<DiscordEmbed>() { value };
            }
        }

        public bool ShouldSerialize_embeds() => _embedParam.Set;

        private readonly DiscordParameter<List<MessageComponent>> _componentParam = new DiscordParameter<List<MessageComponent>>();
        [JsonPropertyName("components")]
        public List<MessageComponent> Components
        {
            get { return _componentParam; }
            set { _componentParam.Value = value; }
        }

        public bool ShouldSerializeComponents() => _componentParam.Set;

        [JsonPropertyName("flags")]
        private int _flags
        {
            get
            {
                return 64;
            }
        }

        [JsonIgnore]
        public bool Ephemeral { get; set; }

        public bool ShouldSerialize_flags() => Ephemeral;
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public class InteractionResponseProperties
    {
        private readonly DiscordParameter<bool> _ttsParam = new DiscordParameter<bool>();
        [JsonProperty("tts")]
        public bool Tts
        {
            get => _ttsParam;
            set => _ttsParam.Value = value;
        }

        public bool ShouldSerializeTts()
        {
            return _ttsParam.Set;
        }

        private readonly DiscordParameter<string> _contentParam = new DiscordParameter<string>();
        [JsonProperty("content")]
        public string Content
        {
            get => _contentParam;
            set => _contentParam.Value = value;
        }

        public bool ShouldSerializeContent()
        {
            return _contentParam.Set;
        }

        private readonly DiscordParameter<List<DiscordEmbed>> _embedParam = new DiscordParameter<List<DiscordEmbed>>();
        [JsonProperty("embeds")]
        private List<DiscordEmbed> _embeds => _embedParam.Value;

        [JsonIgnore]
        public DiscordEmbed Embed
        {
            get => _embedParam.Value?[0];
            set
            {
                if (value == null)
                {
                    _embedParam.Value = null;
                }
                else
                {
                    _embedParam.Value = new List<DiscordEmbed>() { value };
                }
            }
        }

        public bool ShouldSerialize_embeds()
        {
            return _embedParam.Set;
        }

        private readonly DiscordParameter<List<MessageComponent>> _componentParam = new DiscordParameter<List<MessageComponent>>();
        [JsonProperty("components")]
        public List<MessageComponent> Components
        {
            get => _componentParam;
            set => _componentParam.Value = value;
        }

        public bool ShouldSerializeComponents()
        {
            return _componentParam.Set;
        }

        [JsonProperty("flags")]
        private int _flags => 64;

        [JsonIgnore]
        public bool Ephemeral { get; set; }

        public bool ShouldSerialize_flags()
        {
            return Ephemeral;
        }
    }
}

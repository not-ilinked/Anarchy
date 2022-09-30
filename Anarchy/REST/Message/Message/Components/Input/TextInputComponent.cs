using Newtonsoft.Json;

namespace Discord
{
    public class TextInputComponent : MessageInputComponent
    {
        public TextInputComponent()
        {
            Type = MessageComponentType.TextInput;
        }

        [JsonProperty("label")]
        public string Text { get; set; }

        [JsonProperty("min_length")]
        public uint? MinLength { get; set; }

        [JsonProperty("max_length")]
        public uint? MaxLength { get; set; }

        [JsonProperty("placeholder")]
        public string Placeholder { get; set; }

        [JsonProperty("required")]
        public bool Required { get; set; }
    }
}

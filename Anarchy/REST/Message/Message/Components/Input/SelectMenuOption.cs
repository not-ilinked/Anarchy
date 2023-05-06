

using System.Text.Json.Serialization;

namespace Discord
{
    public class SelectMenuOption
    {
        [JsonPropertyName("label")]
        public string Text { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("emoji")]
        public PartialEmoji Emoji { get; set; }

        [JsonPropertyName("default")]
        public bool? Default { get; set; }
    }
}

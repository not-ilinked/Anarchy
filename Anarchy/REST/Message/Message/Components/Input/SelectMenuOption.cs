using Newtonsoft.Json;

namespace Discord
{
    public class SelectMenuOption
    {
        [JsonProperty("label")]
        public string Text { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("emoji")]
        public PartialEmoji Emoji { get; set; }

        [JsonProperty("default")]
        public bool? Default { get; set; }
    }
}

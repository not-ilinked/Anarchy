

using System.Text.Json.Serialization;

namespace Discord
{
    public class CommandOptionChoice
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("value")]
        public object Value { get; set; }
    }
}

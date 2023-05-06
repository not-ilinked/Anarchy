

using System.Text.Json.Serialization;

namespace Discord
{
    public class CrosspostChannel : MinimalTextChannel
    {
        [JsonPropertyName("name")]
        public string Name { get; private set; }
    }
}


using System.Text.Json.Serialization;

namespace Discord
{
    internal class ThreadCreationProperties : ThreadProperties
    {
        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("type")]
        public ChannelType Type { get; set; }
    }
}

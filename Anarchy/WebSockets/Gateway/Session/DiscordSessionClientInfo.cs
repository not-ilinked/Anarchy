

using System.Text.Json.Serialization;
namespace Discord.Gateway
{
    public class DiscordSessionClientInfo
    {
        [JsonPropertyName("version")]
        public int Version { get; private set; }

        [JsonPropertyName("os")]
        public string OS { get; private set; }

        [JsonPropertyName("client")]
        public string ClientType { get; private set; }
    }
}
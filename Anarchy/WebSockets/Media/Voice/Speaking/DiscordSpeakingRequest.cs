

using System.Text.Json.Serialization;

namespace Discord.Media
{
    internal class DiscordSpeakingRequest
    {
        [JsonPropertyName("speaking")]
        public DiscordSpeakingFlags State { get; set; }

        [JsonPropertyName("delay")]
        public int Delay { get; set; }

        [JsonPropertyName("ssrc")]
        public uint SSRC { get; set; }
    }
}

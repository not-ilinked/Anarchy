using Newtonsoft.Json;

namespace Discord.Media
{
    internal class DiscordSpeakingRequest
    {
        [JsonProperty("speaking")]
        public DiscordSpeakingFlags State { get; set; }

        [JsonProperty("delay")]
        public int Delay { get; set; }

        [JsonProperty("ssrc")]
        public uint SSRC { get; set; }
    }
}

using Newtonsoft.Json;

namespace Discord.Media
{
    internal class DiscordSpeakingState
    {
        [JsonProperty("user_id")]
        public ulong? UserId { get; internal set; }

        [JsonProperty("ssrc")]
        public uint SSRC { get; private set; }

        [JsonProperty("speaking")]
        public DiscordSpeakingFlags State { get; private set; }
    }
}

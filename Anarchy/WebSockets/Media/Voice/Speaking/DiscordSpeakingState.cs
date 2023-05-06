

using System.Text.Json.Serialization;

namespace Discord.Media
{
    internal class DiscordSpeakingState
    {
        [JsonPropertyName("user_id")]
        public ulong? UserId { get; internal set; }

        [JsonPropertyName("ssrc")]
        public uint SSRC { get; private set; }

        [JsonPropertyName("speaking")]
        public DiscordSpeakingFlags State { get; private set; }
    }
}

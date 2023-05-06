

using System.Text.Json.Serialization;

namespace Discord.Media
{
    // Opcode: GoLive
    internal class StartStream
    {
        // "guild" for Go Live
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("guild_id")]
        public ulong GuildId { get; set; }

        [JsonPropertyName("channel_id")]
        public ulong ChannelId { get; set; }

        [JsonPropertyName("preferred_region")]
        public string PreferredRegion { get; set; }
    }
}

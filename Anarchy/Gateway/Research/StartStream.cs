using Newtonsoft.Json;

namespace Discord.Gateway.Research
{
    // Opcode: GoLive
    internal class StartStream
    {
        // "guild" for Go Live
        [JsonProperty("type")]
        public string Type { get; set; }


        [JsonProperty("guild_id")]
        public ulong GuildId { get; set; }


        [JsonProperty("channel_id")]
        public ulong ChannelId { get; set; }


        [JsonProperty("preferred_region")]
        public string PreferredRegion { get; set; }
    }
}

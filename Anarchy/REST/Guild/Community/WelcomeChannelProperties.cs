using Newtonsoft.Json;

namespace Discord
{
    public class WelcomeChannelProperties
    {
        [JsonProperty("channel_id")]
        public ulong ChannelId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("emoji_id")]
        public ulong? EmojiId { get; set; }

        [JsonProperty("emoji_name")]
        public string EmojiName { get; set; }
    }
}

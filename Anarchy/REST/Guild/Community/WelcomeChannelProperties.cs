﻿using System.Text.Json.Serialization;

namespace Discord
{
    public class WelcomeChannelProperties
    {
        [JsonPropertyName("channel_id")]
        public ulong ChannelId { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("emoji_id")]
        public ulong? EmojiId { get; set; }

        [JsonPropertyName("emoji_name")]
        public string EmojiName { get; set; }
    }
}

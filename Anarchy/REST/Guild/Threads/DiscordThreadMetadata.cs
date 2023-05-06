using System;
using System.Text.Json.Serialization;

namespace Discord
{
    public class DiscordThreadMetadata
    {
        [JsonPropertyName("archived")]
        public bool Archived { get; private set; }

        [JsonPropertyName("archive_timestamp")]
        public DateTime? ArchivedAt { get; private set; }

        [JsonPropertyName("auto_archive_duration")]
        private readonly int _duration;
        public TimeSpan TTL => new TimeSpan(0, _duration, 0);

        [JsonPropertyName("locked")]
        public bool Locked { get; private set; }

        [JsonPropertyName("create_timestamp")]
        public DateTime? CreatedAt { get; private set; }
    }
}

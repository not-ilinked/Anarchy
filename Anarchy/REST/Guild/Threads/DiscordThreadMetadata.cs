using Newtonsoft.Json;
using System;

namespace Discord
{
    public class DiscordThreadMetadata
    {
        [JsonProperty("archived")]
        public bool Archived { get; private set; }

        [JsonProperty("archive_timestamp")]
        public DateTime? ArchivedAt { get; private set; }

        [JsonProperty("auto_archive_duration")]
        private readonly int _duration;
        public TimeSpan TTL => new TimeSpan(0, _duration, 0);

        [JsonProperty("locked")]
        public bool Locked { get; private set; }

        [JsonProperty("create_timestamp")]
        public DateTime? CreatedAt { get; private set; }
    }
}

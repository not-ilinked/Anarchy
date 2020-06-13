using System;
using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class UserTyping
    {
        [JsonProperty("channel_id")]
        public ulong ChannelId { get; private set; }


        [JsonProperty("guild_id")]
        public ulong? GuildId { get; private set; }


        [JsonProperty("user_id")]
        public ulong UserId { get; private set; }


        [JsonProperty("timestamp")]
#pragma warning disable CS0649
        private readonly ulong _timestamp;
#pragma warning restore CS0649

        [JsonIgnore]
        public DateTimeOffset Timestamp
        {
            get { return DateTimeOffset.FromUnixTimeSeconds((long)_timestamp); }
        }


        public override string ToString()
        {
            return Timestamp.ToString();
        }
    }
}

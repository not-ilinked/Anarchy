using System;
using System.Text.Json.Serialization;

namespace Discord
{
    public class DiscordThreadMember : Controllable
    {
        [JsonPropertyName("id")]
        private readonly ulong _threadId;
        public MinimalTextChannel Thread => new MinimalTextChannel(_threadId).SetClient(Client);

        [JsonPropertyName("user_id")]
        public ulong UserId { get; private set; }

        [JsonPropertyName("join_timestamp")]
        public DateTime JoinedAt { get; private set; }

        [JsonPropertyName("flags")]
        public int Flags { get; private set; }
    }
}

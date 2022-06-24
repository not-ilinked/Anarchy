using System;
using Newtonsoft.Json;

namespace Discord
{
    public class DiscordThreadMember : Controllable
    {
        [JsonProperty("id")]
        private readonly ulong _threadId;
        public MinimalTextChannel Thread => new MinimalTextChannel(_threadId).SetClient(Client);

        [JsonProperty("user_id")]
        public ulong UserId { get; private set; }

        [JsonProperty("join_timestamp")]
        public DateTime JoinedAt { get; private set; }

        [JsonProperty("flags")]
        public int Flags { get; private set; }
    }
}

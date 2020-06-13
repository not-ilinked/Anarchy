using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord.Gateway
{
    class QueryResult
    {
        [JsonProperty("member")]
        public GuildMember Member { get; private set; }
    }

    class Ops
    {
        [JsonProperty("op")]
        public string Opcode { get; private set; }


        [JsonProperty("range")]
        public int[] Range { get; private set; }


        [JsonProperty("items")]
        public List<QueryResult> Items { get; private set; }
    }

    internal class GatewayUserMemberQueryResponse
    {
        [JsonProperty("ops")]
        public List<Ops> Ranges { get; private set; }

        [JsonProperty("online_count")]
        public int OnlineCount { get; private set; }

        [JsonProperty("member_count")]
        public int MemberCount { get; private set; }

        [JsonProperty("guild_id")]
        public ulong GuildId { get; private set; }
    }
}

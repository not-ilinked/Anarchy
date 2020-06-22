using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord.Gateway
{
    internal class MemberListQuery
    {
        public MemberListQuery()
        {
            Channels = new Dictionary<ulong, int[][]>();
        }

        [JsonProperty("guild_id")]
        public ulong GuildId { get; set; }


        [JsonProperty("channels")]
        public Dictionary<ulong, int[][]> Channels { get; set; }
    }
}

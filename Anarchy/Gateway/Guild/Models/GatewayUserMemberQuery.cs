using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord.Gateway
{
    internal class GatewayUserMemberQuery
    {
        public GatewayUserMemberQuery()
        {
            Channels = new Dictionary<ulong, int[][]>();
        }

        [JsonProperty("guild_id")]
        public ulong GuildId { get; set; }


        [JsonProperty("channels")]
        public Dictionary<ulong, int[][]> Channels { get; set; }
    }
}

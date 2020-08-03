using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Discord.Gateway
{
    public class MemberListEventArgs : EventArgs
    {
        [JsonProperty("ops")]
        public List<JObject> Operations { get; private set; }


        [JsonProperty("guild_id")]
        public ulong GuildId { get; private set; }


        [JsonProperty("online_count")]
        public uint Online { get; private set; }


        [JsonProperty("member_count")]
        public uint MemberCount { get; private set; }
    }
}

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Discord.Gateway
{
    public class GuildMemberListEventArgs : EventArgs
    {
        [JsonProperty("guild_id")]
        public ulong GuildId { get; private set; }


        [JsonProperty("ops")]
        public List<JObject> Operations { get; private set; }


        [JsonProperty("online_count")]
        public uint Online { get; private set; }


        [JsonProperty("member_count")]
        public uint Total { get; private set; }


        [JsonProperty("groups")]
        public List<MemberListGroup> Groups { get; private set; }
    }
}

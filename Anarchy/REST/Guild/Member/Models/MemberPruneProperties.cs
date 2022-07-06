using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord
{
    public class MemberPruneProperties
    {
        public MemberPruneProperties()
        {
            IncludedRoles = new List<ulong>();
            Days = 7;
            _computePrunes = true;
        }


        [JsonProperty("compute_prune_count")]
#pragma warning disable IDE0052
        private readonly bool _computePrunes;
#pragma warning restore


        [JsonProperty("days")]
        public uint Days { get; set; }


        [JsonProperty("include_roles")]
        public List<ulong> IncludedRoles { get; set; }
    }
}

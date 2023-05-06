using System.Collections.Generic;
using System.Text.Json.Serialization;

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

        [JsonPropertyName("compute_prune_count")]
#pragma warning disable IDE0052
        private readonly bool _computePrunes;
#pragma warning restore

        [JsonPropertyName("days")]
        public uint Days { get; set; }

        [JsonPropertyName("include_roles")]
        public List<ulong> IncludedRoles { get; set; }
    }
}

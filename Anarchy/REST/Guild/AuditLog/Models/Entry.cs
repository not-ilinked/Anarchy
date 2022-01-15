using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public class AuditLogEntry
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }


        [JsonProperty("target_id")]
        public ulong? TargetId { get; private set; }


        [JsonProperty("changes")]
        public IReadOnlyList<AuditLogChange> Changes { get; private set; }


        [JsonProperty("user_id")]
        public ulong ChangerId { get; private set; }


        [JsonProperty("action_type")]
        public AuditLogActionType Type { get; private set; }


        [JsonProperty("reason")]
        public string Reason { get; private set; }


        public override string ToString()
        {
            return Type.ToString();
        }


        public static implicit operator ulong(AuditLogEntry instance)
        {
            return instance.Id;
        }
    }
}

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord
{
    public class AuditLogEntry
    {
        [JsonPropertyName("id")]
        public ulong Id { get; private set; }

        [JsonPropertyName("target_id")]
        public ulong? TargetId { get; private set; }

        [JsonPropertyName("changes")]
        public IReadOnlyList<AuditLogChange> Changes { get; private set; }

        [JsonPropertyName("user_id")]
        public ulong ChangerId { get; private set; }

        [JsonPropertyName("action_type")]
        public AuditLogActionType Type { get; private set; }

        [JsonPropertyName("reason")]
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

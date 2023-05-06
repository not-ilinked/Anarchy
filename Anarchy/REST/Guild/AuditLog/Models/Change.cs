using System.Text.Json.Serialization;

namespace Discord
{
    public class AuditLogChange
    {
        [JsonPropertyName("new_value")]
        public dynamic NewValue { get; private set; }

        [JsonPropertyName("old_value")]
        public dynamic OldValue { get; private set; }

        [JsonPropertyName("key")]
        public string Key { get; private set; }

        public override string ToString()
        {
            return Key;
        }
    }
}

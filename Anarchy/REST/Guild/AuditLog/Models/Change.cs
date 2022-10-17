using Newtonsoft.Json;

namespace Discord
{
    public class AuditLogChange
    {
        [JsonProperty("new_value")]
        public dynamic NewValue { get; private set; }

        [JsonProperty("old_value")]
        public dynamic OldValue { get; private set; }

        [JsonProperty("key")]
        public string Key { get; private set; }

        public override string ToString()
        {
            return Key;
        }
    }
}

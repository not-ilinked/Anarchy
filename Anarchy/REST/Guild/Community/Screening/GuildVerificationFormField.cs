using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord
{
    public class GuildVerificationFormField
    {
        [JsonPropertyName("field_type")]
        public string FieldType { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("values")]
        public IReadOnlyList<string> Values { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }

        [JsonPropertyName("response")]
        public object Response { get; set; }
    }
}

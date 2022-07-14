using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord
{
    public class GuildVerificationFormField
    {
        [JsonProperty("field_type")]
        public string FieldType { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("values")]
        public IReadOnlyList<string> Values { get; set; }

        [JsonProperty("required")]
        public bool Required { get; set; }

        [JsonProperty("response")]
        public object Response { get; set; }
    }
}

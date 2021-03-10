using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public class GuildVerificationFormField
    {
        [JsonProperty("field_type")]
        public string FieldType { get; private set; }

        [JsonProperty("label")]
        public string Label { get; private set; }

        [JsonProperty("values")]
        public IReadOnlyList<string> Values { get; private set; }

        [JsonProperty("required")]
        public bool Required { get; private set; }

        [JsonProperty("response")]
        public object Response { get; private set; }
    }
}

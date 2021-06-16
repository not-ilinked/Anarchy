using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public class GuildVerificationForm
    {
        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("version")]
        public string Version { get; private set; }

        [JsonProperty("form_fields")]
        public IReadOnlyList<GuildVerificationFormField> Fields { get; private set; }
    }
}

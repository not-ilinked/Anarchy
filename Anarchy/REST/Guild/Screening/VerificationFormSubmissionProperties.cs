using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    internal class VerificationFormSubmissionProperties
    {
        [JsonProperty("version")]
        public string Version { get; set; }


        [JsonProperty("form_fields")]
        public List<GuildVerificationFormField> Fields { get; set; }
    }
}

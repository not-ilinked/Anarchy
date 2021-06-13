using Newtonsoft.Json;
using System;

namespace Discord.Gateway
{
    public class RequiredActionEventArgs : EventArgs
    {
        // REQUIRE_VERIFIED_PHONE
        [JsonProperty("required_action")]
        public string Action { get; private set; }
    }
}

using System;
using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class RequiredActionEventArgs : EventArgs
    {
        // REQUIRE_VERIFIED_PHONE
        [JsonPropertyName("required_action")]
        public string Action { get; private set; }
    }
}

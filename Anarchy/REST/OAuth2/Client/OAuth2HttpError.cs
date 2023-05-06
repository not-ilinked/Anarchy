

using System.Text.Json.Serialization;

namespace Discord
{
    internal class OAuth2HttpError
    {
        [JsonPropertyName("error")]
        public string Error { get; private set; }

        [JsonPropertyName("error_description")]
        public string Description { get; private set; }
    }
}

using Newtonsoft.Json;

namespace Discord
{
    internal class OAuth2HttpError
    {
        [JsonProperty("error")]
        public string Error { get; private set; }

        [JsonProperty("error_description")]
        public string Description { get; private set; }
    }
}

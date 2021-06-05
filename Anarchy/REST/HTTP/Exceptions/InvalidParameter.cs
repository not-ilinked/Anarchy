using Newtonsoft.Json;

namespace Discord
{
    public class InvalidParameter
    {
        [JsonProperty("code")]
        public string Code { get; private set; }

        [JsonProperty("message")]
        public string Message { get; private set; }
    }
}

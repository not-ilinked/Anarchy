using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Discord
{
    public class DiscordHttpError
    {
        [JsonProperty("code")]
        public DiscordError Code { get; private set; }

        [JsonProperty("errors")]
        public JObject Fields { get; private set; }

        [JsonProperty("message")]
        public string Message { get; private set; }

        public DiscordHttpError() { }

        public DiscordHttpError(DiscordError code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}

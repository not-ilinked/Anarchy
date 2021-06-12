using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Discord
{
    internal class DiscordResponse
    {
        [JsonProperty("status")]
        public int Status { get; private set; }

        [JsonProperty("body")]
        public JToken Body { get; private set; }
    }
}

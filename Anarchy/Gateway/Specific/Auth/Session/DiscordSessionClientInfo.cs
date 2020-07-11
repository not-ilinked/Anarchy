using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class DiscordSessionClientInfo
    {
        [JsonProperty("version")]
        public int Version { get; private set; }


        [JsonProperty("os")]
        public string OperatingSystem { get; private set; }


        [JsonProperty("client")]
        public string ClientType { get; private set; }
    }
}

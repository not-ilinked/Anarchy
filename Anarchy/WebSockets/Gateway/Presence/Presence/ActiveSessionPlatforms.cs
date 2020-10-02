using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class ActiveSessionPlatforms
    {
        [JsonProperty("desktop")]
        public UserStatus Desktop { get; private set; }


        [JsonProperty("web")]
        public UserStatus Website { get; private set; }


        [JsonProperty("mobile")]
        public UserStatus Mobile { get; private set; }
    }
}

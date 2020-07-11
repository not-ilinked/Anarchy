using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class MemberListGroup
    {
        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("count")]
        public int Count { get; private set; }
    }
}

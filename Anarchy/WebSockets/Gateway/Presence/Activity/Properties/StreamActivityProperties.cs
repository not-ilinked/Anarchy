using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class StreamActivityProperties : ActivityProperties
    {
        [JsonProperty("type")]
        public new ActivityType Type => ActivityType.Streaming;


        [JsonProperty("url")]
        public string Url { get; set; }
    }
}

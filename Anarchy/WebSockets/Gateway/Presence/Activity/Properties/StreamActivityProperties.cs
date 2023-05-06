

using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class StreamActivityProperties : ActivityProperties
    {
        [JsonPropertyName("type")]
        public new ActivityType Type
        {
            get { return ActivityType.Streaming; }
        }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}

using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class ActivityProperties
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public ActivityType Type { get; set; }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}

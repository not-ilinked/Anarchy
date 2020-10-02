using Newtonsoft.Json;

namespace Discord.Webhook
{
    public class CrosspostChannel : MinimalChannel
    {
        [JsonProperty("name")]
        public string Name { get; private set; }
    }
}

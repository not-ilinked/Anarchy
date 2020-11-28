using Newtonsoft.Json;

namespace Discord
{
    public class CrosspostChannel : MinimalTextChannel
    {
        [JsonProperty("name")]
        public string Name { get; private set; }
    }
}
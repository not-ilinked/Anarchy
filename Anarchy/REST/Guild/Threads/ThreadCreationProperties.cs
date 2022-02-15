using Newtonsoft.Json;

namespace Discord
{
    internal class ThreadCreationProperties : ThreadProperties
    {
        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("type")]
        public ChannelType Type { get; set; }
    }
}

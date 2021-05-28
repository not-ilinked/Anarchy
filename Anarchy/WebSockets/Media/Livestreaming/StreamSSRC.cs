using Newtonsoft.Json;

namespace Discord
{
    public class StreamSSRC
    {
        [JsonProperty("type")]
        public string Type { get; private set; }

        [JsonProperty("ssrc")]
        public uint SSRC { get; private set; }
    }
}

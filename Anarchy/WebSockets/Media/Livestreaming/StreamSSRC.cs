using Newtonsoft.Json;

namespace Discord.Media
{
    public class StreamSSRC
    {
        [JsonProperty("type")]
        public string Type { get; private set; }

        [JsonProperty("ssrc")]
        public uint SSRC { get; private set; }

        [JsonProperty("rtx_ssrc")]
        public uint RtxSsrc { get; private set; }
    }
}



using System.Text.Json.Serialization;

namespace Discord.Media
{
    public class StreamSSRC
    {
        [JsonPropertyName("type")]
        public string Type { get; private set; }

        [JsonPropertyName("ssrc")]
        public uint SSRC { get; private set; }

        [JsonPropertyName("rtx_ssrc")]
        public uint RtxSsrc { get; private set; }
    }
}

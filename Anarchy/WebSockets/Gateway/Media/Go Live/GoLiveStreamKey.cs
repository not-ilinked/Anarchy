using Newtonsoft.Json;

namespace Discord.Media
{
    // Opcode: EndGoLive
    internal class GoLiveStreamKey
    {
        [JsonProperty("stream_key")]
        public string StreamKey { get; set; }
    }
}

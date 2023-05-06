

using System.Text.Json.Serialization;

namespace Discord.Media
{
    // Opcode: EndGoLive
    internal class GoLiveStreamKey
    {
        [JsonPropertyName("stream_key")]
        public string StreamKey { get; set; }
    }
}

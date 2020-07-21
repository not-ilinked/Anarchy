using Newtonsoft.Json;

namespace Discord.Streaming
{
    // Opcode: GoLiveUpdate
    internal class StreamUpdate : GoLiveStreamKey
    {
        [JsonProperty("paused")]
        public bool Paused { get; set; }
    }
}

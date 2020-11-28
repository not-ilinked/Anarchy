using Newtonsoft.Json;

namespace Discord.Media
{
    // Opcode: GoLiveUpdate
    internal class StreamUpdate : GoLiveStreamKey
    {
        [JsonProperty("paused")]
        public bool Paused { get; set; }
    }
}

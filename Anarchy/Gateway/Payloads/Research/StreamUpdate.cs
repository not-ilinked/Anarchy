using Newtonsoft.Json;

namespace Discord.Gateway.Research
{
    // Opcode: GoLiveUpdate
    internal class StreamUpdate : GoLiveStreamKey
    {
        [JsonProperty("paused")]
        public bool Paused { get; set; }
    }
}

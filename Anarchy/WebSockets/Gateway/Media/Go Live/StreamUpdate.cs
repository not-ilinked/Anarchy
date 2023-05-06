

using System.Text.Json.Serialization;
namespace Discord.Media
{
    // Opcode: GoLiveUpdate
    internal class StreamUpdate : GoLiveStreamKey
    {
        [JsonPropertyName("paused")]
        public bool Paused { get; set; }
    }
}

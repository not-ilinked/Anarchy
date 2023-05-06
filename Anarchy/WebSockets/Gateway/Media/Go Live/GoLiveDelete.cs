

using System.Text.Json.Serialization;

namespace Discord.Media
{
    internal class GoLiveDelete : GoLiveStreamKey
    {
        // stream_not_found, stream_ended
        [JsonPropertyName("reason")]
        public string RawReason { get; private set; }
    }
}
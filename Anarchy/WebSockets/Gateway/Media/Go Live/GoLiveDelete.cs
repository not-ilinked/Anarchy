using Newtonsoft.Json;

namespace Discord.Media
{
    internal class GoLiveDelete : GoLiveStreamKey
    {
        // stream_not_found, stream_ended
        [JsonProperty("reason")]
        public string RawReason { get; private set; }
    }
}
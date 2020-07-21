using Newtonsoft.Json;

namespace Discord.Streaming
{
    internal class GoLiveCreate
    {
        [JsonProperty("stream_key")]
        public string StreamKey { get; private set; }


        [JsonProperty("rtc_server_id")]
        public ulong RtcServerId { get; private set; }


        [JsonProperty("region")]
        public string Region { get; private set; }
    }
}

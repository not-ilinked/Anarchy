using Newtonsoft.Json;

namespace Discord.Media
{
    internal class GoLiveCreate : GoLiveUpdate
    {
        [JsonProperty("session_id")]
        public string SessionId { get; private set; }

        [JsonProperty("rtc_server_id")]
        public ulong RtcServerId { get; private set; }
    }
}

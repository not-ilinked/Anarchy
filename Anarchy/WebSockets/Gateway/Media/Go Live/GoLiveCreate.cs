using Newtonsoft.Json;

namespace Discord.Media
{
    internal class GoLiveCreate : GoLiveUpdate
    {
        [JsonProperty("rtc_server_id")]
        public ulong RtcServerId { get; private set; }
    }
}

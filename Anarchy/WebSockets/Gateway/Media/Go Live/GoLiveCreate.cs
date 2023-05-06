

using System.Text.Json.Serialization;

namespace Discord.Media
{
    internal class GoLiveCreate : GoLiveUpdate
    {
        [JsonPropertyName("session_id")]
        public string SessionId { get; private set; }

        [JsonPropertyName("rtc_server_id")]
        public ulong RtcServerId { get; private set; }
    }
}

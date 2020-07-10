using Newtonsoft.Json;

namespace Discord.Gateway.Research
{
    // Opcode: EndGoLive
    internal class GoLiveStreamKey
    {
        // guild:guild_id:channel_id:client_user_id
        [JsonProperty("stream_key")]
        public string StreamKey { get; set; }
    }
}

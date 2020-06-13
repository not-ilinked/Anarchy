using Newtonsoft.Json;

namespace Discord.Voice
{
    public class DiscordVoiceProtocolSelection
    {
        [JsonProperty("protocol")]
        public string Protocol { get; set; }


        [JsonProperty("data")]
        public DiscordVoiceProtocolData ProtocolData { get; set; }
    }
}

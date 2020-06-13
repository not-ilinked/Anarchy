using Newtonsoft.Json;

namespace Discord.Voice
{
    public class DiscordSpeakingRequest
    {
        [JsonProperty("speaking")]
        public DiscordVoiceSpeakingState State { get; set; }


        [JsonProperty("delay")]
        public int Delay { get; set; }


        [JsonProperty("ssrc")]
        public int SSRC { get; set; }
    }
}

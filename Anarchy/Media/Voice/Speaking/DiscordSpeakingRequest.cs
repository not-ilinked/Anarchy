using Newtonsoft.Json;

namespace Discord.Voice
{
    internal class DiscordSpeakingRequest
    {
        [JsonProperty("speaking")]
        public DiscordVoiceSpeakingState State { get; set; }


        [JsonProperty("delay")]
        public int Delay { get; set; }


        [JsonProperty("ssrc")]
        public uint SSRC { get; set; }
    }
}

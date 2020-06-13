using Newtonsoft.Json;

namespace Discord.Voice
{
    public class DiscordVoiceSpeaking
    {
        [JsonProperty("user_id")]
        public ulong UserId { get; private set; }


        [JsonProperty("ssrc")]
        public int SSRC { get; private set; }


        [JsonProperty("speaking")]
        public DiscordVoiceSpeakingState State { get; private set; }
    }
}

using Newtonsoft.Json;

namespace Discord.Media
{
    public class DiscordSSRC
    {
        [JsonProperty("audio_ssrc")]
        public uint Audio { get; set; }


        [JsonProperty("video_ssrc")]
        public uint Video { get; set; }


        [JsonProperty("rtx_ssrc")]
        public uint Rtx { get; set; }
    }
}

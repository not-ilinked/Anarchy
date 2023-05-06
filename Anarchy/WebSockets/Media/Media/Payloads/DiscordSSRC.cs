

using System.Text.Json.Serialization;

namespace Discord.Media
{
    public class DiscordSSRC
    {
        [JsonPropertyName("audio_ssrc")]
        public uint Audio { get; set; }

        [JsonPropertyName("video_ssrc")]
        public uint Video { get; set; }

        [JsonPropertyName("rtx_ssrc")]
        public uint Rtx { get; set; }
    }
}

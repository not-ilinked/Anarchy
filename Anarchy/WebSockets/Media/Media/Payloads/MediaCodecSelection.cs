using Newtonsoft.Json;

namespace Discord.Media
{
    internal class MediaCodecSelection
    {
        [JsonProperty("audio_codec")]
        public string AudioCodec { get; set; }


        [JsonProperty("video_codec")]
        public string VideoCodec { get; set; }
    }
}

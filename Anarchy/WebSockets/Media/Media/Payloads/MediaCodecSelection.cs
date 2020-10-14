using Newtonsoft.Json;

namespace Discord.Media
{
    internal class MediaCodecSelection
    {
        [JsonProperty("media_session_id")]
        internal string NewSessionId { get; private set; }


        [JsonProperty("audio_codec")]
        public string AudioCodec { get; set; }


        [JsonProperty("video_codec")]
        public string VideoCodec { get; set; }
    }
}

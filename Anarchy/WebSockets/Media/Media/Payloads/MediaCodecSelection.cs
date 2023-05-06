

using System.Text.Json.Serialization;

namespace Discord.Media
{
    internal class MediaCodecSelection
    {
        [JsonPropertyName("media_session_id")]
        internal string NewSessionId { get; private set; }

        [JsonPropertyName("audio_codec")]
        public string AudioCodec { get; set; }

        [JsonPropertyName("video_codec")]
        public string VideoCodec { get; set; }
    }
}

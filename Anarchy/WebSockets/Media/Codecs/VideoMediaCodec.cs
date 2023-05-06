

using System.Text.Json.Serialization;
namespace Discord.Media
{
    internal class VideoMediaCodec : MediaCodec
    {
        public VideoMediaCodec()
        {
            Type = CodecType.Video;
        }

        [JsonPropertyName("rtx_payload_type")]
        public int RtxPayloadType { get; set; }
    }
}

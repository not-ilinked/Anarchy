using Newtonsoft.Json;

namespace Discord.Media
{
    internal class VideoMediaCodec : MediaCodec
    {
        public VideoMediaCodec()
        {
            Type = CodecType.Video;
        }

        [JsonProperty("rtx_payload_type")]
        public int RtxPayloadType { get; set; }
    }
}

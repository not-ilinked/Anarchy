using Newtonsoft.Json;

namespace Discord.Voice.Research
{
    // It seems like video_ssrc and rtx_ssrc are chosen by the client.
    internal class SSRCUpdate
    {
        [JsonProperty("audio_ssrc")]
        public uint Audio { get; set; }


        [JsonProperty("video_ssrc")]
        public uint Video { get; set; }


        [JsonProperty("rtx_ssrc")]
        public uint Rtx { get; set; }
    }
}

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord.Media
{
    internal class MediaProtocolSelection
    {
        [JsonPropertyName("protocol")]
        public string Protocol { get; set; }

        [JsonPropertyName("data")]
        public MediaProtocolData ProtocolData { get; set; }

        [JsonPropertyName("rtc_connection_id")]
        public string RtcConnectionId { get; set; }

        [JsonPropertyName("codecs")]
        public List<MediaCodec> Codecs { get; set; }

        [JsonPropertyName("address")]
        public string Host
        {
            get { return ProtocolData.Host; }
        }

        [JsonPropertyName("port")]
        public int Port
        {
            get { return ProtocolData.Port; }
        }
    }
}

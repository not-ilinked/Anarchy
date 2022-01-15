using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord.Media
{
    internal class MediaProtocolSelection
    {
        [JsonProperty("protocol")]
        public string Protocol { get; set; }


        [JsonProperty("data")]
        public MediaProtocolData ProtocolData { get; set; }


        [JsonProperty("rtc_connection_id")]
        public string RtcConnectionId { get; set; }


        [JsonProperty("codecs")]
        public List<MediaCodec> Codecs { get; set; }


        [JsonProperty("address")]
        public string Host => ProtocolData.Host;


        [JsonProperty("port")]
        public int Port => ProtocolData.Port;
    }
}

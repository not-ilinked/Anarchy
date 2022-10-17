using Newtonsoft.Json;

namespace Discord.Media
{
    internal class MediaProtocolData
    {
        [JsonProperty("address")]
        public string Host { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("mode")]
        public string EncryptionMode { get; set; }
    }
}

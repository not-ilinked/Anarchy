

using System.Text.Json.Serialization;
namespace Discord.Media
{
    internal class MediaProtocolData
    {
        [JsonPropertyName("address")]
        public string Host { get; set; }

        [JsonPropertyName("port")]
        public int Port { get; set; }

        [JsonPropertyName("mode")]
        public string EncryptionMode { get; set; }
    }
}

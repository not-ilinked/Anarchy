

using System.Text.Json.Serialization;

namespace Discord
{
    public class DiscordAttachment
    {
        [JsonPropertyName("id")]
        public ulong Id { get; private set; }

        [JsonPropertyName("filename")]
        public string FileName { get; private set; }

        [JsonPropertyName("description")]
        public string Description { get; private set; }

        [JsonPropertyName("content_type")]
        public string ContentType { get; private set; }

        [JsonPropertyName("url")]
        public string Url { get; private set; }

        [JsonPropertyName("proxy_url")]
        public string ProxyUrl { get; private set; }

        [JsonPropertyName("size")]
        public uint FileSize { get; private set; }

        public override string ToString()
        {
            return Url;
        }
    }
}
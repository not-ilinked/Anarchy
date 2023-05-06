using System.Text.Json.Serialization;

namespace Discord
{
    public class EmbedImage
    {
        [JsonPropertyName("url")]
        public string Url { get; internal set; }

        [JsonPropertyName("width")]
        public uint Width { get; private set; }

        [JsonPropertyName("height")]
        public uint Height { get; private set; }

        public override string ToString()
        {
            return $"W: {Width}, H: {Height}";
        }
    }
}

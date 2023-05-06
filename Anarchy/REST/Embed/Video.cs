using System.Text.Json.Serialization;

namespace Discord
{
    //as far as i know videos cannot be put into an embed
    public class EmbedVideo
    {
        [JsonPropertyName("url")]
        public string Url { get; private set; }

        [JsonPropertyName("width")]
        public uint Width { get; private set; }

        [JsonPropertyName("height")]
        public uint Height { get; private set; }

        public override string ToString()
        {
            return $"W: {Width} H: {Height}";
        }
    }
}

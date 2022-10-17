using Newtonsoft.Json;

namespace Discord
{
    public class EmbedImage
    {
        [JsonProperty("url")]
        public string Url { get; internal set; }

        [JsonProperty("width")]
        public uint Width { get; private set; }

        [JsonProperty("height")]
        public uint Height { get; private set; }

        public override string ToString()
        {
            return $"W: {Width}, H: {Height}";
        }
    }
}

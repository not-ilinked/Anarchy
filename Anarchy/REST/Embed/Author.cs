using System.Text.Json.Serialization;

namespace Discord
{
    public class EmbedAuthor
    {
        [JsonPropertyName("name")]
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (value.Length > 256)
                    throw new EmbedException(EmbedError.AuthorNameToolong);

                _name = value;
            }
        }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("icon_url")]
        public string IconUrl { get; set; }

        [JsonPropertyName("proxy_icon_url")]
        public string IconProxyUrl { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

using Newtonsoft.Json;

namespace Discord
{
    public class EmbedAuthor
    {
        [JsonProperty("name")]
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

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }

        [JsonProperty("proxy_icon_url")]
        public string IconProxyUrl { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

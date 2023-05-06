using System.Text.Json.Serialization;

namespace Discord
{
    public class EmbedFooter
    {
        [JsonPropertyName("text")]
        private string _text;

        public string Text
        {
            get { return _text; }
            set
            {
                if (value.Length > 2048)
                    throw new EmbedException(EmbedError.FooterTextTooLong);

                _text = value;
            }
        }

        [JsonPropertyName("icon_url")]
        public string IconUrl { get; set; }

        [JsonPropertyName("proxy_icon_url")]
        public string IconProxyUrl { get; private set; }

        public override string ToString()
        {
            return Text;
        }
    }
}

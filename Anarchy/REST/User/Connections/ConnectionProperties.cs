

using System.Text.Json.Serialization;

namespace Discord
{
    public class ConnectionProperties
    {
        private readonly DiscordParameter<bool> _visibleParameter = new DiscordParameter<bool>();
        [JsonPropertyName("visibility")]
        public bool Visible
        {
            get { return _visibleParameter; }
            set { _visibleParameter.Value = value; }
        }

        public bool ShouldSerializeVisible()
        {
            return _visibleParameter.Set;
        }

        private readonly DiscordParameter<bool> _showParameter = new DiscordParameter<bool>();
        [JsonPropertyName("show_activity")]
        public bool ShowAsActivity
        {
            get { return _showParameter; }
            set { _showParameter.Value = value; }
        }

        public bool ShouldSerializeShowAsActivity()
        {
            return _showParameter.Set;
        }
    }
}

using Newtonsoft.Json;

namespace Discord
{
    public class ConnectionProperties
    {
        private readonly DiscordParameter<bool> _visibleParameter = new DiscordParameter<bool>();
        [JsonProperty("visibility")]
        public bool Visible
        {
            get => _visibleParameter;
            set => _visibleParameter.Value = value;
        }

        public bool ShouldSerializeVisible()
        {
            return _visibleParameter.Set;
        }

        private readonly DiscordParameter<bool> _showParameter = new DiscordParameter<bool>();
        [JsonProperty("show_activity")]
        public bool ShowAsActivity
        {
            get => _showParameter;
            set => _showParameter.Value = value;
        }

        public bool ShouldSerializeShowAsActivity()
        {
            return _showParameter.Set;
        }
    }
}

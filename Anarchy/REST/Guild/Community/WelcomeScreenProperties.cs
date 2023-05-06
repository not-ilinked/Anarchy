using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord
{
    public class WelcomeScreenProperties
    {
        private readonly DiscordParameter<bool> _enabled = new DiscordParameter<bool>();
        [JsonPropertyName("enabled")]
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled.Value = value; }
        }

        public bool ShouldSerializeEnabled()
        {
            return _enabled.Set;
        }

        private readonly DiscordParameter<List<WelcomeChannelProperties>> _channels = new DiscordParameter<List<WelcomeChannelProperties>>();
        [JsonPropertyName("welcome_channels")]
        public List<WelcomeChannelProperties> Channels
        {
            get { return _channels; }
            set { _channels.Value = value; }
        }

        public bool ShouldSerializeChannels()
        {
            return _channels.Set;
        }

        private readonly DiscordParameter<string> _description = new DiscordParameter<string>();
        [JsonPropertyName("description")]
        public string Description
        {
            get { return _description; }
            set { _description.Value = value; }
        }

        public bool ShouldSerializeDescription()
        {
            return _description.Set;
        }
    }
}

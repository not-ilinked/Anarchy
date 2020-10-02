using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public class WelcomeScreenProperties
    {
        private readonly DiscordParameter<bool> _enabled = new DiscordParameter<bool>();
        [JsonProperty("enabled")]
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
        [JsonProperty("welcome_channels")]
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
        [JsonProperty("description")]
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

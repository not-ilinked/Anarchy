using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    public class ChannelSettingsProperties
    {
        [JsonProperty("mute_config")]
#pragma warning disable CS0169
        private readonly GuildMuteConfig _muteConfig;
#pragma warning restore CS0169


        public bool ShouldSerialize_muteConfig()
        {
            return Muted;
        }


        private readonly Property<bool> _mutedProperty = new Property<bool>();
        public bool Muted
        {
            get { return _mutedProperty; }
            set { _mutedProperty.Value = value; }
        }


        public bool ShouldSerializeMuted()
        {
            return _mutedProperty.Set;
        }
    }
}

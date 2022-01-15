using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public class GuildSettingsProperties : ChannelSettingsProperties
    {
        private readonly DiscordParameter<bool> _supressProperty = new DiscordParameter<bool>();
        [JsonProperty("supress_everyone")]
        public bool SupressEveryone
        {
            get => _supressProperty;
            set => _supressProperty.Value = value;
        }


        public bool ShouldSerializeSupressEveryone()
        {
            return _supressProperty.Set;
        }


        private readonly DiscordParameter<bool> _supressRolesProperty = new DiscordParameter<bool>();
        [JsonProperty("supress_roles")]
        public bool SupressRoles
        {
            get => _supressRolesProperty;
            set => _supressRolesProperty.Value = value;
        }


        public bool ShouldSerializeSupressRoles()
        {
            return _supressRolesProperty.Set;
        }


        private readonly DiscordParameter<Dictionary<ulong, ChannelSettingsProperties>> _channelsProperty = new DiscordParameter<Dictionary<ulong, ChannelSettingsProperties>>();
        [JsonProperty("channel_overrides")]
        public Dictionary<ulong, ChannelSettingsProperties> ChannelOverrides
        {
            get => _channelsProperty;
            set => _channelsProperty.Value = value;
        }
    }
}

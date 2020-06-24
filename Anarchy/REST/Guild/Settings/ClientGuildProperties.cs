using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public class ClientGuildProperties : ChannelSettingsProperties
    {
        private readonly Property<ClientGuildNotificationLevel> _notifsProperty = new Property<ClientGuildNotificationLevel>();
        [JsonProperty("message_notifications")]
        public ClientGuildNotificationLevel Notifications
        {
            get { return _notifsProperty; }
            set { _notifsProperty.Value = value; }
        }


        public bool ShouldSerializeNotifications()
        {
            return _notifsProperty.Set;
        }


        private readonly Property<bool> _supressProperty = new Property<bool>();
        [JsonProperty("supress_everyone")]
        public bool SupressEveryone
        {
            get { return _supressProperty; }
            set { _supressProperty.Value = value; }
        }


        public bool ShouldSerializeSupressEveryone()
        {
            return _supressProperty.Set;
        }


        private readonly Property<bool> _supressRolesProperty = new Property<bool>();
        [JsonProperty("supress_roles")]
        public bool SupressRoles
        {
            get { return _supressRolesProperty; }
            set { _supressRolesProperty.Value = value; }
        }


        public bool ShouldSerializeSupressRoles()
        {
            return _supressRolesProperty.Set;
        }


        private readonly Property<Dictionary<ulong, ChannelSettingsProperties>> _channelsProperty = new Property<Dictionary<ulong, ChannelSettingsProperties>>();
        [JsonProperty("channel_overrides")]
        public Dictionary<ulong, ChannelSettingsProperties> ChannelOverrides
        {
            get { return _channelsProperty; }
            set { _channelsProperty.Value = value; }
        }
    }
}

using Newtonsoft.Json;

namespace Discord
{
    public class ClientGuildProperties
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
    }
}

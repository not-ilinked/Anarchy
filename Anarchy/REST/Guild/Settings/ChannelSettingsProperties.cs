using Newtonsoft.Json;

namespace Discord
{
    public class ChannelSettingsProperties
    {
        // cba to implement this rn lol
        [JsonProperty("mute_config")]
#pragma warning disable IDE0052
        private readonly GuildMuteConfig _muteConfig = new GuildMuteConfig() 
        {
            EndTime = null,
            SelectedTimeWindow = -1
        };
#pragma warning restore


        public bool ShouldSerialize_muteConfig()
        {
            return Muted;
        }


        private readonly Property<bool> _mutedProperty = new Property<bool>();
        [JsonProperty("muted")]
        public bool Muted
        {
            get { return _mutedProperty; }
            set { _mutedProperty.Value = value; }
        }


        public bool ShouldSerializeMuted()
        {
            return _mutedProperty.Set;
        }


        private readonly Property<ClientNotificationLevel> _notifsProperty = new Property<ClientNotificationLevel>();
        [JsonProperty("message_notifications")]
        public ClientNotificationLevel Notifications
        {
            get { return _notifsProperty; }
            set { _notifsProperty.Value = value; }
        }


        public bool ShouldSerializeNotifications()
        {
            return _notifsProperty.Set;
        }
    }
}

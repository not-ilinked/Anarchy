using Newtonsoft.Json;

namespace Discord
{
    public class ClientGuildSettings : Controllable
    {
        [JsonProperty("guild_id")]
        private ulong? _guildId;

        public MinimalGuild Guild
        {
            get
            {
                if (_guildId.HasValue)
                    return new MinimalGuild(_guildId.Value).SetClient(Client);
                else
                    return null;
            }
        }


        [JsonProperty("message_notifications")]
        public ClientGuildNotificationLevel Notifications { get; private set; }


        [JsonProperty("supress_everyone")]
        public bool SupressEveryone { get; private set; }


        [JsonProperty("supress_roles")]
        public bool SupressRoles { get; private set; }


        [JsonProperty("mobile_push")]
        public bool MobilePushNotifications { get; private set; }
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public class ClientGuildSettings : DiscordChannelSettings
    {
        [JsonProperty("guild_id")]
        private readonly ulong? _guildId;

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


        [JsonProperty("channel_overrides")]
        public IReadOnlyList<DiscordChannelSettings> ChannelOverrides { get; private set; }
    }
}

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord
{
    public class ClientGuildSettings : Controllable
    {
        [JsonPropertyName("guild_id")]
        internal ulong? GuildId { get; private set; }

        public MinimalGuild Guild
        {
            get
            {
                return new MinimalGuild(GuildId.Value).SetClient(Client);
            }
        }

        [JsonPropertyName("muted")]
        public bool Muted { get; private set; }

        [JsonPropertyName("message_notifications")]
        public ClientNotificationLevel Notifications { get; private set; }

        [JsonPropertyName("supress_everyone")]
        public bool SupressEveryone { get; private set; }

        [JsonPropertyName("supress_roles")]
        public bool SupressRoles { get; private set; }

        [JsonPropertyName("mobile_push")]
        public bool MobilePushNotifications { get; private set; }

        [JsonPropertyName("channel_overrides")]
        public IReadOnlyList<DiscordChannelSettings> ChannelOverrides { get; private set; }
    }
}

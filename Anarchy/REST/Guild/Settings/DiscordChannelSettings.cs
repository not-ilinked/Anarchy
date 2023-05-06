

using System.Text.Json.Serialization;

namespace Discord
{
    public class DiscordChannelSettings
    {
        [JsonPropertyName("channel_id")]
        public ulong Id { get; private set; }

        [JsonPropertyName("muted")]
        public bool Muted { get; private set; }

        [JsonPropertyName("message_notifications")]
        public ClientNotificationLevel Notifications { get; private set; }
    }
}

using Newtonsoft.Json;

namespace Discord
{
    public class DiscordChannelSettings
    {
        [JsonProperty("channel_id")]
        public ulong Id { get; private set; }


        [JsonProperty("muted")]
        public bool Muted { get; private set; }


        [JsonProperty("message_notifications")]
        public ClientNotificationLevel Notifications { get; private set; }
    }
}

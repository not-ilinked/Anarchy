using System;
using System.Text.Json.Serialization;

namespace Discord
{
    public class DiscordBoostSlot : Controllable
    {
        public DiscordBoostSlot()
        {
            OnClientUpdated += (sender, e) => GuildSubscription.SetClient(Client);
        }

        [JsonPropertyName("id")]
        public ulong Id { get; private set; }

        [JsonPropertyName("subscription_id")]
        public ulong ActiveSubscriptionId { get; private set; }

        [JsonPropertyName("premium_guild_subscription")]
        public DiscordGuildSubscription GuildSubscription { get; private set; }

        [JsonPropertyName("canceled")]
        public bool Canceled { get; private set; }

        [JsonPropertyName("cooldown_ends_at")]
        public DateTime? Cooldown { get; private set; }

        // TODO: Get an endpoint for cancelling
    }
}

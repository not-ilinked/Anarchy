using Newtonsoft.Json;
using System;

namespace Discord
{
    public class DiscordGuildBoost : Controllable
    {
        public DiscordGuildBoost()
        {
            OnClientUpdated += (sender, e) =>
            {
                if (GuildSubscription != null)
                    GuildSubscription.SetClient(Client);
            };
        }

        [JsonProperty("id")]
        public ulong Id { get; private set; }


        [JsonProperty("subscription_id")]
        public ulong ActiveSubscriptionId { get; private set; }


        [JsonProperty("premium_guild_subscription")]
        public DiscordGuildSubscription GuildSubscription { get; private set; }


        [JsonProperty("canceled")]
        public bool Canceled { get; private set; }


        [JsonProperty("cooldown_ends_at")]
        public DateTime? Cooldown { get; private set; }

        // TODO: Get an endpoint for cancelling
    }
}

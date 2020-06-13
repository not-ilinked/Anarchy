using Newtonsoft.Json;
using System;

namespace Discord
{
    public class DiscordNitroBoost : Controllable
    {
        public DiscordNitroBoost()
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
        public ulong SubscriptionId { get; private set; }


        [JsonProperty("premium_guild_subscription")]
        public DiscordGuildSubscription GuildSubscription { get; private set; }


        [JsonProperty("canceled")]
        public bool Canceled { get; private set; }


        [JsonProperty("cooldown_ends_at")]
#pragma warning disable CS0649
        private readonly string _cooldown;
#pragma warning restore CS0649

        public DateTime? Cooldown
        {
            get
            {
                if (_cooldown == null)
                    return null;
                else
                    return DiscordTimestamp.FromString(_cooldown);
            }
        }


        public static implicit operator ulong(DiscordNitroBoost instance)
        {
            return instance.Id;
        }
    }
}

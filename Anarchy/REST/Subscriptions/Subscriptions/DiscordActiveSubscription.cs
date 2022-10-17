using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord
{
    public class DiscordActiveSubscription
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }

        [JsonProperty("type")]
        public int Type { get; private set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; private set; }

        [JsonProperty("currency")]
        public string Currency { get; private set; }

        [JsonProperty("status")]
        public DiscordSubscriptionStatus Status { get; private set; }

        [JsonProperty("payment_source_id")]
        public ulong? PaymentMethodId { get; private set; }

        [JsonProperty("payment_gateway_plan_id")]
        public string SubscriptionTier { get; private set; }

        [JsonProperty("plan_id")]
        public ulong PrimaryPlanId { get; private set; }

        [JsonProperty("items")]
        public IReadOnlyList<DiscordSubscription> Subscriptions { get; private set; }
    }
}

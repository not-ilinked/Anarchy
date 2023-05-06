using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord
{
    public class DiscordActiveSubscription
    {
        [JsonPropertyName("id")]
        public ulong Id { get; private set; }

        [JsonPropertyName("type")]
        public int Type { get; private set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; private set; }

        [JsonPropertyName("currency")]
        public string Currency { get; private set; }

        [JsonPropertyName("status")]
        public DiscordSubscriptionStatus Status { get; private set; }

        [JsonPropertyName("payment_source_id")]
        public ulong? PaymentMethodId { get; private set; }

        [JsonPropertyName("payment_gateway_plan_id")]
        public string SubscriptionTier { get; private set; }

        [JsonPropertyName("plan_id")]
        public ulong PrimaryPlanId { get; private set; }

        [JsonPropertyName("items")]
        public IReadOnlyList<DiscordSubscription> Subscriptions { get; private set; }
    }
}

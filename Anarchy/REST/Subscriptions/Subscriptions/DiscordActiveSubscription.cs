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
        private string _createdAt;


        public DateTime SubscribedAt
        {
            get { return DiscordTimestamp.FromString(_createdAt); }
        }


        [JsonProperty("status")]
        public DiscordSubscriptionStatus Status { get; private set; }


        [JsonProperty("payment_source_id")]
        public ulong? PaymentMethodId { get; private set; }


        [JsonProperty("payment_gateway_plan_id")]
        public string GatewayPlanId { get; private set; }


        [JsonProperty("plan_id")]
        public ulong PlanId { get; private set; }


        [JsonProperty("items")]
        public IReadOnlyList<DiscordSubscription> Subscriptions { get; private set; }


        [JsonProperty("currency")]
        public string Currency { get; private set; }
    }
}

using System.Text.Json.Serialization;

namespace Discord
{
    public class DiscordGift : Controllable
    {
        [JsonPropertyName("id")]
        public ulong Id { get; private set; }

        [JsonPropertyName("sku_id")]
        public ulong SkuId { get; private set; }

        [JsonPropertyName("application_id")]
        public ulong ApplicationId { get; private set; }

        [JsonPropertyName("user")]
        public DiscordUser Gifter { get; private set; }

        [JsonPropertyName("consumed")]
        public bool Consumed { get; private set; }

        [JsonPropertyName("subscription_plan")]
        public SubscriptionPlan SubscriptionPlan { get; private set; }
    }
}

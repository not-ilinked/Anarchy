using Newtonsoft.Json;

namespace Discord
{
    public class DiscordGift
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }


        [JsonProperty("sku_id")]
        public ulong SkuId { get; private set; }


        [JsonProperty("application_id")]
        public ulong ApplicationId { get; private set; }


        [JsonProperty("user_id")]
        public ulong GifterId { get; private set; }


        [JsonProperty("consumed")]
        public bool Consumed { get; private set; }


        [JsonProperty("subscription_plan")]
        public NitroSubscriptionPlan SubscriptionPlan { get; private set; }
    }
}

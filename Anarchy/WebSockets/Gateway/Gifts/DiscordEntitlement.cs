

using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    // missing research (see REST/Gifts/Extensions.cs)
    public class DiscordEntitlement
    {
        // user_id is a thing, not sure why; it appears to just be the local user's ID

        [JsonPropertyName("type")]
        public int Type { get; private set; } // missing research

        [JsonPropertyName("subscription_plan")]
        public SubscriptionPlan SubscriptionPlan { get; private set; }

        [JsonPropertyName("sku_id")]
        public ulong SkuId { get; private set; }

        // not entirely sure what this is, but my guess is that since
        // some gifts can be claimed multiple times, this might be the gift's ID
        [JsonPropertyName("parent_id")]
        public ulong? ParentId { get; private set; }

        [JsonPropertyName("id")]
        public ulong Id { get; private set; }

        [JsonPropertyName("gifter_user_id")]
        public ulong GifterId { get; private set; }

        // not sure how they would need their own flag, but sure discord, whatever...
        [JsonPropertyName("deleted")]
        public bool Deleted { get; private set; }

        [JsonPropertyName("consumed")]
        public bool Consumed { get; private set; }

        [JsonPropertyName("application_id")]
        public ulong ApplicationId { get; private set; }
    }
}

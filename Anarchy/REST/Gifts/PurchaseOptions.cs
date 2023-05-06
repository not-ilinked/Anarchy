using System.Text.Json.Serialization;

namespace Discord
{
    public class PurchaseOptions
    {
        [JsonPropertyName("expected_amount")]
        public int ExpectedAmount { get; set; }

        [JsonPropertyName("gift")]
#pragma warning disable CS0414
        private readonly bool _gift = true; // rn we only have support for gifts kek
#pragma warning restore CS0414

        [JsonPropertyName("payment_source_id")]
        public ulong PaymentMethodId { get; set; }

        [JsonPropertyName("sku_subscription_plan_id")]
        public ulong SkuPlanId { get; set; }
    }
}

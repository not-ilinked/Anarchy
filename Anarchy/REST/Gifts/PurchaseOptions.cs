using Newtonsoft.Json;

namespace Discord
{
    public class PurchaseOptions
    {
        [JsonProperty("expected_amount")]
        public int ExpectedAmount { get; set; }


        [JsonProperty("gift")]
#pragma warning disable CS0414
        private readonly bool _gift = true; // rn we only have support for gifts kek
#pragma warning restore CS0414


        [JsonProperty("payment_source_id")]
        public ulong PaymentMethodId { get; set; }


        [JsonProperty("sku_subscription_plan_id")]
        public ulong SkuPlanId { get; set; }
    }
}

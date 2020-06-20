using Newtonsoft.Json;

namespace Discord
{
    public class SubscriptionPlan
    {
        [JsonProperty("sku_id")]
        public ulong SkuId { get; private set; }


        [JsonProperty("name")]
        public string Name { get; private set; }


        [JsonProperty("currency")]
        public string Currency { get; private set; }


        [JsonProperty("price")]
        public int Price { get; private set; }


        [JsonProperty("tax_inclusive")]
        public bool TaxInclusive { get; private set; }


        [JsonProperty("id")]
        public ulong Id { get; private set; }


        public static implicit operator ulong(SubscriptionPlan instance)
        {
            return instance.Id;
        }
    }
}

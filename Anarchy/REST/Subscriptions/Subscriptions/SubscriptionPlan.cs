

using System.Text.Json.Serialization;

namespace Discord
{
    public class SubscriptionPlan
    {
        [JsonPropertyName("sku_id")]
        public ulong SkuId { get; private set; }

        [JsonPropertyName("name")]
        public string Name { get; private set; }

        [JsonPropertyName("currency")]
        public string Currency { get; private set; }

        [JsonPropertyName("price")]
        public int Price { get; private set; }

        [JsonPropertyName("tax_inclusive")]
        public bool TaxInclusive { get; private set; }

        [JsonPropertyName("id")]
        public ulong Id { get; private set; }

        public static implicit operator ulong(SubscriptionPlan instance)
        {
            return instance.Id;
        }
    }
}

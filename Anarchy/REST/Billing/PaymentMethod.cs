using Newtonsoft.Json;

namespace Discord
{
    public class PaymentMethod : ControllableEx
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }


        [JsonProperty("type")]
        public PaymentMethodType Type { get; private set; }


        [JsonProperty("invalid")]
        public bool Invalid { get; private set; }


        [JsonProperty("billing_address")]
        public BillingAddress BillingAddress { get; private set; }


        [JsonProperty("country")]
        public string County { get; private set; }


        [JsonProperty("default")]
        public bool Default { get; private set; }


        public string PurchaseGift(ulong skuId, ulong subPlanId, int expectedAmount)
        {
            return Client.PurchaseGift(Id, skuId, subPlanId, expectedAmount);
        }


        public string PurchaseNitroGift(DiscordNitroSubType nitroType)
        {
            return Client.PurchaseNitroGift(Id, nitroType);
        }


        public static implicit operator ulong(PaymentMethod instance)
        {
            return instance.Id;
        }
    }
}

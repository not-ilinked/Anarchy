using Newtonsoft.Json;
using System.Threading.Tasks;

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


        public async Task<string> PurchaseGiftAsync(ulong skuId, ulong subPlanId, int expectedAmount)
        {
            return await Client.PurchaseGiftAsync(Id, skuId, subPlanId, expectedAmount);
        }

        public string PurchaseGift(ulong skuId, ulong subPlanId, int expectedAmount)
        {
            return PurchaseGiftAsync(skuId, subPlanId, expectedAmount).Result;
        }


        public async Task<string> PurchaseNitroGiftAsync(DiscordNitroSubType nitroType)
        {
            return await Client.PurchaseNitroGiftAsync(Id, nitroType);
        }

        public string PurchaseNitroGift(DiscordNitroSubType nitroType)
        {
            return PurchaseNitroGiftAsync(nitroType).Result;
        }
            

        public static implicit operator ulong(PaymentMethod instance)
        {
            return instance.Id;
        }
    }
}

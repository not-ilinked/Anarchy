using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Discord
{
    public class PaymentMethod : Controllable
    {
        [JsonPropertyName("id")]
        public ulong Id { get; private set; }

        [JsonPropertyName("type")]
        public PaymentMethodType Type { get; private set; }

        [JsonPropertyName("invalid")]
        public bool Invalid { get; private set; }

        [JsonPropertyName("billing_address")]
        public BillingAddress BillingAddress { get; private set; }

        [JsonPropertyName("country")]
        public string County { get; private set; }

        [JsonPropertyName("default")]
        public bool Default { get; private set; }

        public async Task<string> PurchaseGiftAsync(ulong skuId, ulong subPlanId, int expectedAmount)
        {
            return await Client.PurchaseGiftAsync(Id, skuId, subPlanId, expectedAmount);
        }

        public string PurchaseGift(ulong skuId, ulong subPlanId, int expectedAmount)
        {
            return PurchaseGiftAsync(skuId, subPlanId, expectedAmount).GetAwaiter().GetResult();
        }

        public async Task<string> PurchaseNitroGiftAsync(DiscordNitroSubType nitroType)
        {
            return await Client.PurchaseNitroGiftAsync(Id, nitroType);
        }

        public string PurchaseNitroGift(DiscordNitroSubType nitroType)
        {
            return PurchaseNitroGiftAsync(nitroType).GetAwaiter().GetResult();
        }
    }
}
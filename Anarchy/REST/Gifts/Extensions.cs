using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord
{
    // Anarchy's gift support might be a little shakey right now
    // Things may have been changed (or i've ignored them for whatever reason), so i'll patch everything up once i have data
    public static class GiftsExtensions
    {
        
        public static async Task<string> PurchaseGiftAsync(this DiscordClient client, ulong paymentMethodId, ulong skuId, ulong subPlanId, int expectedAmount)
        {
            return (await client.HttpClient.PostAsync($"/store/skus/{skuId}/purchase", new PurchaseOptions()
            {
                PaymentMethodId = paymentMethodId,
                SkuPlanId = subPlanId,
                ExpectedAmount = expectedAmount
            })).Deserialize<JObject>().Value<string>("gift_code");
        }

        public static string PurchaseGift(this DiscordClient client, ulong paymentMethodId, ulong skuId, ulong subPlanId, int expectedAmount)
        {
            return client.PurchaseGiftAsync(paymentMethodId, skuId, subPlanId, expectedAmount).GetAwaiter().GetResult();
        }


        public static async Task<IReadOnlyList<DiscordGift>> GetGiftInventoryAsync(this DiscordClient client)
        {
            return (await client.HttpClient.GetAsync("/users/@me/entitlements/gifts")).Deserialize<IReadOnlyList<DiscordGift>>();
        }

        public static IReadOnlyList<DiscordGift> GetGiftInventory(this DiscordClient client)
        {
            return client.GetGiftInventoryAsync().GetAwaiter().GetResult();
        }


        public static async Task<IReadOnlyList<RedeemableDiscordGift>> QueryGiftCodesAsync(this DiscordClient client, ulong skuId, ulong subPlanId)
        {
            return (await client.HttpClient.GetAsync($"/users/@me/entitlements/gift-codes?sku_id={skuId}&subscription_plan_id={subPlanId}"))
                                .Deserialize<IReadOnlyList<RedeemableDiscordGift>>().SetClientsInList(client);
        }

        public static IReadOnlyList<RedeemableDiscordGift> QueryGiftCodes(this DiscordClient client, ulong skuId, ulong subPlanId)
        {
            return client.QueryGiftCodesAsync(skuId, subPlanId).GetAwaiter().GetResult();
        }


        public static async Task<RedeemableDiscordGift> CreateGiftCodeAsync(this DiscordClient client, ulong skuId, ulong subPlanId)
        {
            return (await client.HttpClient.PostAsync("/users/@me/entitlements/gift-codes", $"{{\"sku_id\":{skuId},\"subscription_plan_id\":{subPlanId}}}"))
                                .Deserialize<RedeemableDiscordGift>().SetClient(client);
        }

        public static RedeemableDiscordGift CreateGiftCode(this DiscordClient client, ulong skuId, ulong subPlanId)
        {
            return client.CreateGiftCodeAsync(skuId, subPlanId).GetAwaiter().GetResult();
        }


        public static async Task RevokeGiftCodeAsync(this DiscordClient client, string code)
        {
            await client.HttpClient.DeleteAsync("/@me/entitlements/gift-codes/" + code);
        }

        public static void RevokeGiftCode(this DiscordClient client, string code)
        {
            client.RevokeGiftCodeAsync(code).GetAwaiter().GetResult();
        }


        public static async Task RedeemGiftAsync(this DiscordClient client, string code, ulong? channelId = null)
        {
            await client.HttpClient.PostAsync($"/entitlements/gift-codes/{code}/redeem", channelId.HasValue ? $"{{\"channel_id\":{channelId.Value}}}" : null);
        }

        public static void RedeemGift(this DiscordClient client, string code, ulong? channelId = null)
        {
            client.RedeemGiftAsync(code, channelId).GetAwaiter().GetResult();
        }


        public static async Task<DiscordGift> GetGiftAsync(this DiscordClient client, string code)
        {
            return (await client.HttpClient.GetAsync($"/entitlements/gift-codes/{code}?with_application=false&with_subscription_plan=true"))
                                .Deserialize<DiscordGift>();
        }

        public static DiscordGift GetGift(this DiscordClient client, string code)
        {
            return client.GetGiftAsync(code).GetAwaiter().GetResult();
        }


        public static async Task<string> PurchaseNitroGiftAsync(this DiscordClient client, ulong paymentMethodId, DiscordNitroSubType type)
        {
            return await client.PurchaseGiftAsync(paymentMethodId, type.SkuId, type.SubscriptionPlanId, type.ExpectedAmount);
        }

        public static string PurchaseNitroGift(this DiscordClient client, ulong paymentMethodId, DiscordNitroSubType type)
        {
            return client.PurchaseGift(paymentMethodId, type.SkuId, type.SubscriptionPlanId, type.ExpectedAmount);
        }
    }
}

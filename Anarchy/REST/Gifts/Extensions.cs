using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Discord
{
    public static class GiftsExtensions
    {
        public static string PurchaseGift(this DiscordClient client, ulong paymentMethodId, ulong skuId, ulong subPlanId, int expectedAmount)
        {
            return client.HttpClient.Post($"https://discordapp.com/api/v6/store/skus/{skuId}/purchase", new PurchaseOptions()
            {
                PaymentMethodId = paymentMethodId,
                SkuPlanId = subPlanId,
                ExpectedAmount = expectedAmount
            }).Deserialize<JObject>().Value<string>("gift_code");
        }


        public static IReadOnlyList<DiscordGift> GetGiftInventory(this DiscordClient client)
        {
            return client.HttpClient.Get("/users/@me/entitlements/gifts").Deserialize<IReadOnlyList<DiscordGift>>();
        }


        public static IReadOnlyList<DiscordGiftCode> QueryGiftCodes(this DiscordClient client, ulong skuId, ulong subPlanId)
        {
            return client.HttpClient.Get($"/users/@me/entitlements/gift-codes?sku_id={skuId}&subscription_plan_id={subPlanId}")
                                .Deserialize<IReadOnlyList<DiscordGiftCode>>().SetClientsInList(client);
        }


        public static DiscordGiftCode CreateGiftCode(this DiscordClient client, ulong skuId, ulong subPlanId)
        {
            return client.HttpClient.Post("/users/@me/entitlements/gift-codes", $"{{\"sku_id\":{skuId},\"subscription_plan_id\":{subPlanId}}}")
                                .Deserialize<DiscordGiftCode>().SetClient(client);
        }


        public static void RevokeGiftCode(this DiscordClient client, string code)
        {
            client.HttpClient.Delete("/@me/entitlements/gift-codes/" + code);
        }


        public static void RedeemGift(this DiscordClient client, string code, ulong? channelId = null)
        {
            client.HttpClient.Post($"/entitlements/gift-codes/{code}/redeem", channelId.HasValue ? $"{{\"channel_id\":{channelId.Value}}}" : null);
        }


        public static DiscordNitroGift GetNitroGift(this DiscordClient client, string code)
        {
            return client.HttpClient.Get($"/entitlements/gift-codes/{code}?with_application=false&with_subscription_plan=true")
                                .Deserialize<DiscordNitroGift>();
        }


        public static string PurchaseNitroGift(this DiscordClient client, ulong paymentMethodId, DiscordNitroSubType type)
        {
            return client.PurchaseGift(paymentMethodId, type.SkuId, type.SubscriptionPlanId, type.ExpectedAmount);
        }
    }
}

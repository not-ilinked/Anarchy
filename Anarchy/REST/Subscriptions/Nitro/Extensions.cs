using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord
{
    public static class NitroSubscriptionExtensions
    {
        public static async Task<IReadOnlyList<DiscordGuildSubscription>> BoostGuildAsync(this DiscordClient client, ulong guildId, IEnumerable<ulong> boosts)
        {
            return (await client.HttpClient.PutAsync($"/guilds/{guildId}/premium/subscriptions", $"{{\"user_premium_guild_subscription_slot_ids\":{JsonConvert.SerializeObject(boosts)}}}"))
                                .Deserialize<IReadOnlyList<DiscordGuildSubscription>>().SetClientsInList(client);
        }

        public static IReadOnlyList<DiscordGuildSubscription> BoostGuild(this DiscordClient client, ulong guildId, IEnumerable<ulong> boosts)
        {
            return client.BoostGuildAsync(guildId, boosts).Result;
        }


        public static async Task RemoveGuildBoostAsync(this DiscordClient client, ulong guildId, ulong subscriptionId)
        {
            await client.HttpClient.DeleteAsync($"/guilds/{guildId}/premium/subscriptions/{subscriptionId}");
        }

        public static void RemoveGuildBoost(this DiscordClient client, ulong guildId, ulong subscriptionId)
        {
            client.RemoveGuildBoostAsync(guildId, subscriptionId).GetAwaiter().GetResult();
        }


        public static async Task<IReadOnlyList<DiscordNitroBoost>> GetNitroBoostsAsync(this DiscordClient client)
        {
            return (await client.HttpClient.GetAsync("/users/@me/guilds/premium/subscription-slots")).Deserialize<List<DiscordNitroBoost>>().SetClientsInList(client);
        }

        public static IReadOnlyList<DiscordNitroBoost> GetNitroBoosts(this DiscordClient client)
        {
            return client.GetNitroBoostsAsync().Result;
        }


        public static async Task<DiscordActiveSubscription> PurchaseGuildBoostAsync(this DiscordClient client, ulong paymentMethodId, int quantity = 1)
        {
            var activeSubs = await client.GetActiveSubscriptionsAsync();

            if (activeSubs.Count > 0)
                return await client.AddPlanToSubscriptionAsync(paymentMethodId, activeSubs[0].Id, DiscordNitroSubTypes.GuildBoost.SubscriptionPlanId, quantity);
            else
            {
                return await client.PurchaseSubscriptionAsync(paymentMethodId, DiscordNitroSubTypes.GuildBoost.SkuId, new List<AdditionalSubscriptionPlan>()
                {
                    new AdditionalSubscriptionPlan()
                    {
                        Id = DiscordNitroSubTypes.GuildBoost.SubscriptionPlanId,
                        Quantity = quantity
                    }
                });
            }
        }

        public static DiscordActiveSubscription PurchaseGuildBoost(this DiscordClient client, ulong paymentMethodId, int quantity = 1)
        {
            return client.PurchaseGuildBoostAsync(paymentMethodId, quantity).Result;
        }
    }
}

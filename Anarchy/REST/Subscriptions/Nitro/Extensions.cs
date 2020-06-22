using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public static class NitroSubscriptionExtensions
    {
        public static IReadOnlyList<DiscordGuildSubscription> BoostGuild(this DiscordClient client, ulong guildId, IEnumerable<ulong> boosts)
        {
            return client.HttpClient.Put($"/guilds/{guildId}/premium/subscriptions", $"{{\"user_premium_guild_subscription_slot_ids\":{JsonConvert.SerializeObject(boosts)}}}")
                                .Deserialize<IReadOnlyList<DiscordGuildSubscription>>().SetClientsInList(client);
        }


        public static void RemoveGuildBoost(this DiscordClient client, ulong guildId, ulong subscriptionId)
        {
            client.HttpClient.Delete($"/guilds/{guildId}/premium/subscriptions/{subscriptionId}");
        }


        public static IReadOnlyList<DiscordNitroBoost> GetNitroBoosts(this DiscordClient client)
        {
            return client.HttpClient.Get("/users/@me/guilds/premium/subscription-slots").Deserialize<List<DiscordNitroBoost>>().SetClientsInList(client);
        }


        public static DiscordActiveSubscription PurchaseGuildBoost(this DiscordClient client, ulong paymentMethodId, int quantity = 1)
        {
            var activeSubs = client.GetActiveSubscriptions();

            if (activeSubs.Count > 0)
                return client.AddPlanToSubscription(paymentMethodId, activeSubs[0].Id, DiscordNitroSubTypes.GuildBoost.SubscriptionPlanId, quantity);
            else
            {
                return client.PurchaseSubscription(paymentMethodId, DiscordNitroSubTypes.GuildBoost.SkuId, new List<AdditionalSubscriptionPlan>()
                {
                    new AdditionalSubscriptionPlan()
                    {
                        Id = DiscordNitroSubTypes.GuildBoost.SubscriptionPlanId,
                        Quantity = quantity
                    }
                });
            }
        }
    }
}

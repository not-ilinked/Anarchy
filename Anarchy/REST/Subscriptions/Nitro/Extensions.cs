using System.Collections.Generic;
using System.Threading.Tasks;


namespace Discord
{
    public static class NitroSubscriptionExtensions
    {
        public static async Task<IReadOnlyList<DiscordGuildSubscription>> BoostGuildAsync(this DiscordClient client, ulong guildId, IEnumerable<ulong> boostSlots)
        {
            return (await client.HttpClient.PutAsync($"/guilds/{guildId}/premium/subscriptions", $"{{\"user_premium_guild_subscription_slot_ids\":{JsonConvert.SerializeObject(boostSlots)}}}"))
                                .Deserialize<IReadOnlyList<DiscordGuildSubscription>>().SetClientsInList(client);
        }

        public static IReadOnlyList<DiscordGuildSubscription> BoostGuild(this DiscordClient client, ulong guildId, IEnumerable<ulong> boosts)
        {
            return client.BoostGuildAsync(guildId, boosts).GetAwaiter().GetResult();
        }

        public static async Task RemoveGuildBoostAsync(this DiscordClient client, ulong guildId, ulong subscriptionId)
        {
            await client.HttpClient.DeleteAsync($"/guilds/{guildId}/premium/subscriptions/{subscriptionId}");
        }

        public static void RemoveGuildBoost(this DiscordClient client, ulong guildId, ulong subscriptionId)
        {
            client.RemoveGuildBoostAsync(guildId, subscriptionId).GetAwaiter().GetResult();
        }

        public static async Task<IReadOnlyList<DiscordBoostSlot>> GetBoostSlotsAsync(this DiscordClient client)
        {
            return (await client.HttpClient.GetAsync("/users/@me/guilds/premium/subscription-slots")).Deserialize<List<DiscordBoostSlot>>().SetClientsInList(client);
        }

        public static IReadOnlyList<DiscordBoostSlot> GetBoostSlots(this DiscordClient client)
        {
            return client.GetBoostSlotsAsync().GetAwaiter().GetResult();
        }

        public static async Task<DiscordActiveSubscription> SetAdditionalBoostsAsync(this DiscordClient client, ulong paymentMethodId, ulong activeSubscriptionId, uint amount)
        {
            string plan = JsonConvert.SerializeObject(new AdditionalSubscriptionPlan() { Id = DiscordNitroSubTypes.GuildBoost.SubscriptionPlanId, Quantity = (int) amount });

            return (await client.HttpClient.PatchAsync("/users/@me/billing/subscriptions/" + activeSubscriptionId, $"{{\"payment_source_id\":{paymentMethodId},\"additional_plans\":[{plan}]}}")).Deserialize<DiscordActiveSubscription>();
        }

        public static DiscordActiveSubscription SetAdditionalBoosts(this DiscordClient client, ulong paymentMethodId, ulong activeSubscriptionId, uint amount)
        {
            return client.SetAdditionalBoostsAsync(paymentMethodId, activeSubscriptionId, amount).GetAwaiter().GetResult();
        }
    }
}

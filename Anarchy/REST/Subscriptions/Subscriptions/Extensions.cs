using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord
{
    public static class SubscriptionExtensions
    {
        public static async Task<IReadOnlyList<DiscordActiveSubscription>> GetActiveSubscriptionsAsync(this DiscordClient client)
        {
            return (await client.HttpClient.GetAsync("/users/@me/billing/subscriptions"))
                                        .Deserialize<IReadOnlyList<DiscordActiveSubscription>>();
        }

        public static IReadOnlyList<DiscordActiveSubscription> GetActiveSubscriptions(this DiscordClient client)
        {
            return client.GetActiveSubscriptionsAsync().Result;
        }


        public static async Task<DiscordActiveSubscription> AddPlanToSubscriptionAsync(this DiscordClient client, ulong paymentMethodId, ulong subscriptionId, ulong planId, int quantity = 1)
        {
            string plan = JsonConvert.SerializeObject(new AdditionalSubscriptionPlan() { Id = planId, Quantity = quantity });

            return (await client.HttpClient.PatchAsync("/users/@me/billing/subscriptions/" + subscriptionId, $"{{\"payment_source_id\":{paymentMethodId},\"additional_plans\":[{plan}]}}")).Deserialize<DiscordActiveSubscription>();
        }

        public static DiscordActiveSubscription AddPlanToSubscription(this DiscordClient client, ulong paymentMethodId, ulong subscriptionId, ulong planId, int quantity = 1)
        {
            return client.AddPlanToSubscriptionAsync(paymentMethodId, subscriptionId, planId, quantity).Result;
        }


        public static async Task<DiscordActiveSubscription> PurchaseSubscriptionAsync(this DiscordClient client, ulong paymentMethodId, ulong skuId, List<AdditionalSubscriptionPlan> additionalPlans = null)
        {
            string addPlans = additionalPlans == null ? null : JsonConvert.SerializeObject(additionalPlans);

            return (await client.HttpClient.PostAsync("/users/@me/billing/subscriptions", $"{{\"plan_id\":{skuId},\"payment_source_id\":{paymentMethodId},\"additional_plans\":{addPlans}}}"))
                                .Deserialize<DiscordActiveSubscription>();
        }

        public static DiscordActiveSubscription PurchaseSubscription(this DiscordClient client, ulong paymentMethodId, ulong skuId, List<AdditionalSubscriptionPlan> additionalPlans = null)
        {
            return client.PurchaseSubscriptionAsync(paymentMethodId, skuId, additionalPlans).Result;
        }
    }
}

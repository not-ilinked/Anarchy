using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public static class SubscriptionExtensions
    {
        public static IReadOnlyList<DiscordActiveSubscription> GetActiveSubscriptions(this DiscordClient client)
        {
            return client.HttpClient.Get("/users/@me/billing/subscriptions")
                                        .Deserialize<IReadOnlyList<DiscordActiveSubscription>>();
        }


        public static DiscordActiveSubscription AddPlanToSubscription(this DiscordClient client, ulong paymentMethodId, ulong subscriptionId, ulong planId, int quantity = 1)
        {
            string plan = JsonConvert.SerializeObject(new AdditionalSubscriptionPlan() { Id = planId, Quantity = quantity });

            return client.HttpClient.Patch("/users/@me/billing/subscriptions/" + subscriptionId, $"{{\"payment_source_id\":{paymentMethodId},\"additional_plans\":[{plan}]}}").Deserialize<DiscordActiveSubscription>();
        }


        public static DiscordActiveSubscription PurchaseSubscription(this DiscordClient client, ulong paymentMethodId, ulong skuId, List<AdditionalSubscriptionPlan> additionalPlans = null)
        {
            string addPlans = additionalPlans == null ? null : JsonConvert.SerializeObject(additionalPlans);

            return client.HttpClient.Post("/users/@me/billing/subscriptions", $"{{\"plan_id\":{skuId},\"payment_source_id\":{paymentMethodId},\"additional_plans\":{addPlans}}}")
                                .Deserialize<DiscordActiveSubscription>();
        }
    }
}

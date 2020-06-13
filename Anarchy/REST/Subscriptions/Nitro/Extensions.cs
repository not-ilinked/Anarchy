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
    }
}

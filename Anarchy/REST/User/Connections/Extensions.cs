using System;
using System.Collections.Generic;
using System.Linq;

namespace Discord
{
    public static class ConnectedAccountsExtensions
    {
        public static IReadOnlyList<ConnectedAccount> GetConnectedAccounts(this DiscordClient client)
        {
            return client.HttpClient.Get($"/users/@me/connections")
                                .Deserialize<IReadOnlyList<ConnectedAccount>>().SetClientsInList(client);
        }


        public static void RemoveConnectedAccount(this DiscordClient client, AccountType type, string id)
        {
            client.HttpClient.Delete($"/users/@me/connections/{type}/{id}");
        }
    }
}

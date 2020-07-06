using System.Collections.Generic;

namespace Discord
{
    public static class ConnectedAccountsExtensions
    {
        public static IReadOnlyList<ClientConnectedAccount> GetConnectedAccounts(this DiscordClient client)
        {
            return client.HttpClient.Get($"/users/@me/connections")
                                .Deserialize<IReadOnlyList<ClientConnectedAccount>>().SetClientsInList(client);
        }


        public static void RemoveConnectedAccount(this DiscordClient client, AccountType type, string id)
        {
            client.HttpClient.Delete($"/users/@me/connections/{type}/{id}");
        }
    }
}

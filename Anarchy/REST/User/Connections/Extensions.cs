using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord
{
    public static class ConnectedAccountsExtensions
    {
        public static async Task<IReadOnlyList<ClientConnectedAccount>> GetConnectedAccountsAsync(this DiscordClient client)
        {
            return (await client.HttpClient.GetAsync($"/users/@me/connections"))
                                .Deserialize<IReadOnlyList<ClientConnectedAccount>>().SetClientsInList(client);
        }

        public static IReadOnlyList<ClientConnectedAccount> GetConnectedAccounts(this DiscordClient client)
        {
            return client.GetConnectedAccountsAsync().Result;
        }


        public static async Task RemoveConnectedAccountAsync(this DiscordClient client, AccountType type, string id)
        {
            await client.HttpClient.DeleteAsync($"/users/@me/connections/{type}/{id}");
        }

        public static void RemoveConnectedAccount(this DiscordClient client, AccountType type, string id)
        {
            client.RemoveConnectedAccountAsync(type, id).GetAwaiter().GetResult();
        }
    }
}

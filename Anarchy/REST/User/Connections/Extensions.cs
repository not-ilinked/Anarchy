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


        public static ConnectedAccount AddConnectedAccount(this DiscordClient client, AccountType type, string name, bool visibleToPublic = true)
        {
            string id = RandomString(12); // instead of having to GET the connections every time to find the next id, let's just pass a random one /shrug

            return client.HttpClient.Put($"/users/@me/connections/{type.ToString().ToLower()}/{id}", $"{{\"name\":\"{name}\",\"visibility\":{(visibleToPublic ? 1 : 0)}}}")
                                                        .Deserialize<ConnectedAccount>().SetClient(client);
        }


        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

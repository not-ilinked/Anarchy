using System.Threading.Tasks;

namespace Discord
{
    public static class UserExtensions
    {
        public static async Task<DiscordUser> GetUserAsync(this DiscordClient client, ulong userId)
        {
            return (await client.HttpClient.GetAsync($"/users/{userId}")).Deserialize<DiscordUser>().SetClient(client);
        }

        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="userId">ID of the user</param>
        public static DiscordUser GetUser(this DiscordClient client, ulong userId)
        {
            return client.GetUserAsync(userId).Result;
        }


        public static async Task<DiscordClientUser> GetClientUserAsync(this DiscordClient client)
        {
            try
            {
                return client.User = (await client.HttpClient.GetAsync("/users/@me")).Deserialize<DiscordClientUser>().SetClient(client);
            }
            catch (DiscordHttpException)
            {
                client.User = null;
                throw;
            }
        }

        /// <summary>
        /// Gets the account's user
        /// </summary>
        public static DiscordClientUser GetClientUser(this DiscordClient client)
        {
            return client.GetClientUserAsync().Result;
        }
    }
}
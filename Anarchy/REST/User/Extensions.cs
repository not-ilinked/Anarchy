namespace Discord
{
    public static class UserExtensions
    {
        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="userId">ID of the user</param>
        public static DiscordUser GetUser(this DiscordClient client, ulong userId)
        {
            return client.HttpClient.Get($"/users/{userId}").Deserialize<DiscordUser>().SetClient(client);
        }


        /// <summary>
        /// Gets the account's user
        /// </summary>
        public static DiscordClientUser GetClientUser(this DiscordClient client)
        {
            try
            {
                client.User = client.HttpClient.Get("/users/@me")
                                        .Deserialize<DiscordClientUser>().SetClient(client);
                return client.User;
            }
            catch (DiscordHttpException)
            {
                client.User = null;
                throw;
            }
        }
    }
}
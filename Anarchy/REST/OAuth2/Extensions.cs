using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Discord
{
    public static class OAuth2Extensions
    {
        public static IReadOnlyList<AuthorizedApp> GetAuthorizedApps(this DiscordClient client)
        {
            return client.HttpClient.Get($"/oauth2/tokens")
                                .Deserialize<IReadOnlyList<AuthorizedApp>>().SetClientsInList(client);
        }


        public static void DeauthorizeApp(this DiscordClient client, ulong appId)
        {
            client.HttpClient.Delete("/oauth2/tokens/" + appId);
        }


        /// <summary>
        /// Adds a bot to a server
        /// </summary>
        /// <param name="botId">client_id from the oauth2 url</param>
        /// <param name="guildId">the guild to add the bot to</param>
        /// <param name="permissions">permissions the bot should have</param>
        /// <param name="captchaKey">captcha key used to validate the request</param>
        public static void AuthorizeBot(this DiscordClient client, ulong botId, ulong guildId, DiscordPermission permissions, string captchaKey)
        {
            client.HttpClient.Post($"/oauth2/authorize?client_id={botId}&scope=bot", JsonConvert.SerializeObject(new DiscordBotAuthProperties()
            {
                GuildId = guildId,
                Permissions = permissions,
                CaptchaKey = captchaKey
            }));
        }


        /// <summary>
        /// Authorizes an app to a client
        /// </summary>
        /// <param name="appId">client_id from the oauth2 url</param>
        /// <param name="scope">scope from the oauth2 url</param>
        /// <returns>A redirect url containing the auth code</returns>
        public static string AuthorizeApp(this DiscordClient client, ulong appId, string scope)
        {
            return client.HttpClient.Post($"/oauth2/authori ze?client_id={appId}&response_type=code&scope={scope}").Deserialize<JObject>().Value<string>("location");
        }


        /// <summary>
        /// Gets all OAuth2 applications the client owns
        /// </summary>
        public static IReadOnlyList<OAuth2Application> GetApplications(this DiscordClient client)
        {
            return client.HttpClient.Get("/applications?with_team_applications=true")
                                .Deserialize<IReadOnlyList<OAuth2Application>>().SetClientsInList(client);
        }


        /// <summary>
        /// Gets an OAuth2 application owned by the client
        /// </summary>
        /// <param name="id">The application's ID</param>
        public static OAuth2Application GetApplication(this DiscordClient client, ulong id)
        {
            return client.HttpClient.Get("/applications/" + id).Deserialize<OAuth2Application>().SetClient(client);
        }


        /// <summary>
        /// Creates an OAuth2 application
        /// </summary>
        /// <param name="name">name for the application</param>
        public static OAuth2Application CreateApplication(this DiscordClient client, string name)
        {
            return client.HttpClient.Post("/oauth2/applications", $"{{\"name\":\"{name}\"}}")
                                .Deserialize<OAuth2Application>().SetClient(client);
        }


        /// <summary>
        /// Modifies an OAuth2 application
        /// </summary>
        /// <param name="id">The application's ID</param>
        /// <param name="properties">Your changes</param>
        public static OAuth2Application ModifyApplication(this DiscordClient client, ulong id, DiscordApplicationProperties properties)
        {
            return client.HttpClient.Patch("/applications/" + id, properties).Deserialize<OAuth2Application>().SetClient(client);
        }


        /// <summary>
        /// Adds a bot to the application
        /// </summary>
        /// <param name="appId">ID of the OAuth2 application</param>
        /// <returns></returns>
        public static ApplicationBot AddBotToApplication(this DiscordClient client, ulong appId)
        {
            return client.HttpClient.Post($"/oauth2/applications/{appId}/bot").Deserialize<ApplicationBot>().SetClient(client);
        }


        /// <summary>
        /// Deletes an OAuth2 application
        /// </summary>
        /// <param name="appId">ID of the application</param>
        public static void DeleteApplication(this DiscordClient client, ulong appId)
        {
            client.HttpClient.Delete($"/oauth2/applications/{appId}");
        }
    }
}

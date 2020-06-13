using Newtonsoft.Json.Linq;
using System;

namespace Discord
{
    public static class AuthExtensions
    { 
        /// <summary>
        /// Logs into an account by a username and password
        /// </summary>
        public static void LoginToAccount(this DiscordClient client, string email, string password, string captchaKey = null)
        {
            client.Token = client.HttpClient.Post("/auth/login", new LoginRequest()
            {
                Email = email,
                Password = password,
                CaptchaKey = captchaKey
            }).Deserialize<JObject>().Value<string>("token");
        }


        /// <summary>
        /// Registers an account
        /// </summary>
        /// <param name="registration">Info about registration</param>
        public static void RegisterAccount(this DiscordClient client, DiscordRegistration registration)
        {
            registration.Fingerprint = client.HttpClient.Fingerprint;

            client.Token = client.HttpClient.Post("/auth/register", registration).Deserialize<JObject>().Value<string>("token");
        }


        /// <summary>
        /// Sends a password reset request to the client's email
        /// </summary>
        public static void RequestPasswordReset(this DiscordClient client, string email = null)
        {
            if (email == null)
            {
                if (client.User == null)
                    throw new InvalidOperationException("Client is not logged into an account and therefore requires the email parameter");

                email = client.User.Email;
            }

            client.HttpClient.Post("/auth/forgot", $"{{\"email\":\"{email}\"}}");
        }
    }
}

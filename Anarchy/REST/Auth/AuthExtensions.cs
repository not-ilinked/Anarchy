using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Discord
{
    public static class AuthExtensions
    { 
        public static async Task LoginToAccountAsync(this DiscordClient client, string email, string password, string captchaKey = null)
        {
            client.Token = (await client.HttpClient.PostAsync("/auth/login", new LoginRequest()
            {
                Email = email,
                Password = password,
                CaptchaKey = captchaKey
            })).Deserialize<JObject>().Value<string>("token");
        }

        /// <summary>
        /// Logs into an account by a username and password
        /// </summary>
        public static void LoginToAccount(this DiscordClient client, string email, string password, string captchaKey = null)
        {
            client.LoginToAccountAsync(email, password, captchaKey).GetAwaiter().GetResult();
        }



        public static async Task RegisterAccountAsync(this DiscordClient client, DiscordRegistration registration)
        {
            registration.Fingerprint = client.HttpClient.Fingerprint;

            client.Token = (await client.HttpClient.PostAsync("/auth/register", registration)).Deserialize<JObject>().Value<string>("token");
        }

        /// <summary>
        /// Registers an account
        /// </summary>
        /// <param name="registration">Info about registration</param>
        public static void RegisterAccount(this DiscordClient client, DiscordRegistration registration)
        {
            client.RegisterAccountAsync(registration).GetAwaiter().GetResult();
        }



        public static async Task RequestPasswordResetAsync(this DiscordClient client, string email = null)
        {
            if (email == null)
            {
                if (client.User == null)
                    throw new InvalidOperationException("Client is not logged into an account and therefore requires the email parameter");

                email = client.User.Email;
            }

            await client.HttpClient.PostAsync("/auth/forgot", $"{{\"email\":\"{email}\"}}");
        }


        /// <summary>
        /// Sends a password reset request to the client's email
        /// </summary>
        public static void RequestPasswordReset(this DiscordClient client, string email = null)
        {
            client.RequestPasswordResetAsync(email).GetAwaiter().GetResult();
        }
    }
}

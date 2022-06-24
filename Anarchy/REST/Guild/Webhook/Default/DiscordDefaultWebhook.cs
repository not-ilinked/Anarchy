using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Discord
{
    public class DiscordDefaultWebhook : DiscordWebhook
    {
        [JsonProperty("token")]
        public string Token { get; private set; }

        public DiscordDefaultWebhook() : base()
        { }

        public DiscordDefaultWebhook(ulong webhookId, string token) : this()
        {
            Client = new DiscordClient();
            Update((DiscordDefaultWebhook)Client.GetWebhook(webhookId, token));
        }


        private void Update(DiscordDefaultWebhook hook)
        {
            Token = hook.Token;
            base.Update(hook);
        }


        public new async Task UpdateAsync()
        {
            Update(await Client.GetWebhookAsync(Id, Token));
        }

        /// <summary>
        /// Updates the webhook's info
        /// </summary>
        public new void Update()
        {
            UpdateAsync().GetAwaiter().GetResult();
        }


        public new async Task DeleteAsync()
        {
            await Client.DeleteWebhookAsync(Id, Token);
        }

        /// <summary>
        /// Deletes the webhook
        /// </summary>
        public new void Delete()
        {
            DeleteAsync().GetAwaiter().GetResult();
        }


        public async Task SendMessageAsync(string content, DiscordEmbed embed = null, DiscordWebhookProfile profile = null)
        {
            await Client.SendWebhookMessageAsync(Id, Token, content, embed, profile);
        }

        /// <summary>
        /// Sends a message through the webhook
        /// </summary>
        /// <param name="content">The message to send</param>
        /// <param name="embed">Embed to include in the message</param>
        /// <param name="profile">Custom Username and Avatar url (both are optional)</param>
        public void SendMessage(string content, DiscordEmbed embed = null, DiscordWebhookProfile profile = null)
        {
            SendMessageAsync(content, embed, profile).GetAwaiter().GetResult();
        }
    }
}

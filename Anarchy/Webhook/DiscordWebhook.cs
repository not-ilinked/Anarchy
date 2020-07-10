using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Discord.Webhook
{
    public class DiscordWebhook : Controllable
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }


        [JsonProperty("type")]
        public DiscordWebhookType Type { get; private set; }


        [JsonProperty("name")]
        public string Name { get; private set; }


        [JsonProperty("avatar")]
        private string _avatarHash;

        public DiscordUserAvatar Avatar
        {
            get
            {
                return new DiscordUserAvatar(Id, _avatarHash);
            }
        }


        [JsonProperty("user")]
        public DiscordUser Creator { get; private set; }


        [JsonProperty("token")]
        public string Token { get; private set; }


        [JsonProperty("channel_id")]
        public ulong ChannelId { get; private set; }


        [JsonProperty("guild_id")]
        public ulong GuildId { get; private set; }


        private void Update(DiscordWebhook hook)
        {
            Name = hook.Name;
            _avatarHash = hook.Avatar.Hash;
            Creator = hook.Creator;
            ChannelId = hook.ChannelId;
        }


        public async Task UpdateAsync()
        {
            Update(await Client.GetWebhookAsync(Id, Token));
        }

        /// <summary>
        /// Updates the webhook's info
        /// </summary>
        public void Update()
        {
            UpdateAsync().GetAwaiter().GetResult();
        }

        public async Task ModifyAsync(DiscordWebhookProperties properties)
        {
            Update(await Client.ModifyWebhookAsync(Id, properties));
        }

        /// <summary>
        /// Modifies the webhook
        /// </summary>
        /// <param name="properties">Options for modifying the webhook</param>
        public void Modify(DiscordWebhookProperties properties)
        {
            ModifyAsync(properties).GetAwaiter().GetResult();
        }


        public async Task DeleteAsync()
        {
            await Client.DeleteWebhookAsync(Id, Token);
        }

        /// <summary>
        /// Deletes the webhook
        /// </summary>
        public void Delete()
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


        public override string ToString()
        {
            return Name;
        }


        public static implicit operator ulong(DiscordWebhook instance)
        {
            return instance.Id;
        }
    }
}

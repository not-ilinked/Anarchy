using Newtonsoft.Json;
using System;
using System.Drawing;

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
        private string _avatarId;

        public DiscordUserAvatarCDNImage Avatar
        {
            get
            {
                return new DiscordUserAvatarCDNImage(Id, _avatarId);
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


        /// <summary>
        /// Updates the webhook's info
        /// </summary>
        public void Update()
        {
            DiscordWebhook hook = Client.GetWebhook(Id, Token);
            Name = hook.Name;
            _avatarId = hook.Avatar.Hash;
            Creator = hook.Creator;
            ChannelId = hook.ChannelId;
        }


        /// <summary>
        /// Modifies the webhook
        /// </summary>
        /// <param name="properties">Options for modifying the webhook</param>
        public void Modify(DiscordWebhookProperties properties)
        {
            DiscordWebhook hook = Client.ModifyWebhook(Id, properties);
            Name = hook.Name;
            _avatarId = hook.Avatar.Hash;
            ChannelId = hook.ChannelId;
        }


        /// <summary>
        /// Deletes the webhook
        /// </summary>
        public void Delete()
        {
            Client.DeleteChannelWebhook(Id, Token);
        }


        /// <summary>
        /// Sends a message through the webhook
        /// </summary>
        /// <param name="content">The message to send</param>
        /// <param name="embed">Embed to include in the message</param>
        /// <param name="profile">Custom Username and Avatar url (both are optional)</param>
        public void SendMessage(string content, DiscordEmbed embed = null, DiscordWebhookProfile profile = null)
        {
            Client.SendWebhookMessage(Id, Token, content, embed, profile);
        }


        /// <summary>
        /// Gets the webhook's avatar
        /// </summary>
        /// <returns>The avatar (returns null if AvatarId is null)</returns>
        [Obsolete("GetAvatar is obsolete. Use Avatar.Download() instead")]
        public Image GetAvatar()
        {
            return null;
        }


        public override string ToString()
        {
            return Name;
        }


        public static implicit operator ulong(DiscordWebhook instance)
        {
            return instance.Id;
        }


        public static DiscordWebhook FromUrl(string webhookUrl)
        {
            string[] info = webhookUrl.Substring(webhookUrl.IndexOf("https://discordapp.com/api/webhooks/") + 35).Split('/');

            return new DiscordClient(new DiscordConfig() { GetFingerprint = false }).GetWebhook(ulong.Parse(info[0]), info[1]);
        }
    }
}

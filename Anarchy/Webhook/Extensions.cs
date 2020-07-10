using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord.Webhook
{
    public static class WebhookExtensions
    {
        [Obsolete("CreateChannelWebhookAsync is depricated. Call CreateWebhookAsync instead", true)]
        public static Task<DiscordWebhook> CreateChannelWebhookAsync(this DiscordClient client, ulong channelId, DiscordWebhookProperties properties)
        {
            return null;
        }

        public static async Task<DiscordWebhook> CreateWebhookAsync(this DiscordClient client, ulong channelId, DiscordWebhookProperties properties)
        {
            properties.ChannelId = channelId;
            DiscordWebhook hook = (await client.HttpClient.PostAsync($"/channels/{channelId}/webhooks", properties)).Deserialize<DiscordWebhook>().SetClient(client);
            hook.Modify(properties);
            return hook;
        }


        [Obsolete("CreateChannelWebhook is depricated. Call CreateWebhook instead", true)]
        public static DiscordWebhook CreateChannelWebhook(this DiscordClient client, ulong channelId, DiscordWebhookProperties properties)
        {
            return null;
        }

        /// <summary>
        /// Creates a webhook
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="properties">Options for creating/modifying the webhook</param>
        /// <returns>The created webhook</returns>
        public static DiscordWebhook CreateWebhook(this DiscordClient client, ulong channelId, DiscordWebhookProperties properties)
        {
            return client.CreateWebhookAsync(channelId, properties).Result;
        }


        public static async Task<DiscordWebhook> ModifyWebhookAsync(this DiscordClient client, ulong webhookId, DiscordWebhookProperties properties)
        {
            return (await client.HttpClient.PatchAsync($"/webhooks/{webhookId}", properties)).Deserialize<DiscordWebhook>().SetClient(client);
        }

        /// <summary>
        /// Modifies a webhook
        /// </summary>
        /// <param name="webhookId">ID of the webhook</param>
        /// <param name="properties">Options for modifyiing a webhook</param>
        /// <returns>The modified webhook</returns>
        public static DiscordWebhook ModifyWebhook(this DiscordClient client, ulong webhookId, DiscordWebhookProperties properties)
        {
            return client.ModifyWebhookAsync(webhookId, properties).Result;
        }


        public static async Task SendWebhookMessageAsync(this DiscordClient client, ulong webhookId, string webhookToken, string content, DiscordEmbed embed = null, DiscordWebhookProfile profile = null)
        {
            var properties = new WebhookMessageProperties() { Content = content, Embed = embed };

            if (profile != null)
            {
                if (profile.NameProperty.Set)
                    properties.Username = profile.Username;
                if (profile.AvatarProperty.Set)
                    properties.AvatarUrl = profile.AvatarUrl;
            }

            await client.HttpClient.PostAsync($"/webhooks/{webhookId}/{webhookToken}", properties);
        }

        /// <summary>
        /// Sends a message through the webhook
        /// </summary>
        /// <param name="webhookId">ID of the webhook</param>
        /// <param name="webhookToken">The webhook's token</param>
        /// <param name="content">The message to send</param>
        /// <param name="embed">Embed to include in the message</param>
        /// <param name="profile">Custom Username and Avatar url (both are optional)</param>
        public static void SendWebhookMessage(this DiscordClient client, ulong webhookId, string webhookToken, string content, DiscordEmbed embed = null, DiscordWebhookProfile profile = null)
        {
            client.SendWebhookMessageAsync(webhookId, webhookToken, content, embed, profile).GetAwaiter().GetResult();
        }


        [Obsolete("DeleteChannelWebhookAsync is depricated. Call DeleteWebhookAsync instead", true)]
        public static Task DeleteChannelWebhookAsync(this DiscordClient client, ulong webhookId, string token = null)
        {
            return null;
        }

        public static async Task DeleteWebhookAsync(this DiscordClient client, ulong webhookId, string token = null)
        {
            await client.HttpClient.DeleteAsync($"/webhooks/{webhookId}/{token}");
        }

        [Obsolete("DeleteChannelWebhookAsync is depricated. Call DeleteWebhookAsync instead", true)]
        public static void DeleteChannelWebhook(this DiscordClient client, ulong webhookId, string token = null)
        {

        }

        /// <summary>
        /// Deletes a webhook
        /// </summary>
        /// <param name="webhookId">ID of the webhook</param>
        public static void DeleteWebhook(this DiscordClient client, ulong webhookId, string token = null)
        {
            client.DeleteWebhookAsync(webhookId, token).GetAwaiter().GetResult();
        }


        public static async Task<DiscordWebhook> GetWebhookAsync(this DiscordClient client, ulong webhookId, string token = "")
        {
            return (await client.HttpClient.GetAsync($"/webhooks/{webhookId}/{token}"))
                            .Deserialize<DiscordWebhook>().SetClient(client);
        }

        /// <summary>
        /// Gets a webhook (if <paramref name="token"/> is set the client does not have to be authenticated)
        /// </summary>
        /// <param name="webhookId">ID of the webhook</param>
        /// <param name="token">The webhooks's token</param>
        public static DiscordWebhook GetWebhook(this DiscordClient client, ulong webhookId, string token = "")
        {
            return client.GetWebhookAsync(webhookId, token).Result;
        }


        public static async Task<IReadOnlyList<DiscordWebhook>> GetGuildWebhooksAsync(this DiscordClient client, ulong guildId)
        {
            return (await client.HttpClient.GetAsync($"/guilds/{guildId}/webhooks"))
                                .Deserialize<IReadOnlyList<DiscordWebhook>>().SetClientsInList(client);
        }

        /// <summary>
        /// Gets a guild's webhooks
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static IReadOnlyList<DiscordWebhook> GetGuildWebhooks(this DiscordClient client, ulong guildId)
        {
            return client.GetGuildWebhooksAsync(guildId).Result;
        }


        public static async Task<IReadOnlyList<DiscordWebhook>> GetChannelWebhooksAsync(this DiscordClient client, ulong channelId)
        {
            return (await client.HttpClient.GetAsync($"/channels/{channelId}/webhooks"))
                                .Deserialize<IReadOnlyList<DiscordWebhook>>().SetClientsInList(client);
        }

        /// <summary>
        /// Gets a channel's webhooks
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        public static IReadOnlyList<DiscordWebhook> GetChannelWebhooks(this DiscordClient client, ulong channelId)
        {
            return client.GetChannelWebhooksAsync(channelId).Result;
        }
    }
}

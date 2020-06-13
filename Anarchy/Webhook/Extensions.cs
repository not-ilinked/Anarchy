using System.Collections.Generic;

namespace Discord.Webhook
{
    public static class WebhookExtensions
    {
        /// <summary>
        /// Creates a webhook
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="properties">Options for creating/modifying the webhook</param>
        /// <returns>The created webhook</returns>
        public static DiscordWebhook CreateChannelWebhook(this DiscordClient client, ulong channelId, DiscordWebhookProperties properties)
        {
            properties.ChannelId = channelId;
            DiscordWebhook hook = client.HttpClient.Post($"/channels/{channelId}/webhooks", properties).Deserialize<DiscordWebhook>().SetClient(client);
            hook.Modify(properties);
            return hook;
        }


        /// <summary>
        /// Modifies a webhook
        /// </summary>
        /// <param name="webhookId">ID of the webhook</param>
        /// <param name="properties">Options for modifyiing a webhook</param>
        /// <returns>The modified webhook</returns>
        public static DiscordWebhook ModifyWebhook(this DiscordClient client, ulong webhookId, DiscordWebhookProperties properties)
        {
            return client.HttpClient.Patch($"/webhooks/{webhookId}", properties).Deserialize<DiscordWebhook>().SetClient(client);
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
            var properties = new WebhookMessageProperties() { Content = content, Embed = embed };

            if (profile != null)
            {
                if (profile.NameProperty.Set)
                    properties.Username = profile.Username;
                if (profile.AvatarProperty.Set)
                    properties.AvatarUrl = profile.AvatarUrl;
            }

            client.HttpClient.Post($"/webhooks/{webhookId}/{webhookToken}", properties);
        }


        /// <summary>
        /// Deletes a webhook
        /// </summary>
        /// <param name="webhookId">ID of the webhook</param>
        public static void DeleteChannelWebhook(this DiscordClient client, ulong webhookId, string token = null)
        {
            client.HttpClient.Delete($"/webhooks/{webhookId}/{token}");
        }


        /// <summary>
        /// Gets a webhook (if <paramref name="token"/> is set the client does not have to be authenticated)
        /// </summary>
        /// <param name="webhookId">ID of the webhook</param>
        /// <param name="token">The webhooks's token</param>
        public static DiscordWebhook GetWebhook(this DiscordClient client, ulong webhookId, string token = "")
        {
            return client.HttpClient.Get($"/webhooks/{webhookId}/{token}")
                            .Deserialize<DiscordWebhook>().SetClient(client);
        }


        /// <summary>
        /// Gets a guild's webhooks
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static IReadOnlyList<DiscordWebhook> GetGuildWebhooks(this DiscordClient client, ulong guildId)
        {
            return client.HttpClient.Get($"/guilds/{guildId}/webhooks")
                                .Deserialize<IReadOnlyList<DiscordWebhook>>().SetClientsInList(client);
        }


        /// <summary>
        /// Gets a channel's webhooks
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        public static IReadOnlyList<DiscordWebhook> GetChannelWebhooks(this DiscordClient client, ulong channelId)
        {
            return client.HttpClient.Get($"/channels/{channelId}/webhooks")
                                .Deserialize<IReadOnlyList<DiscordWebhook>>().SetClientsInList(client);
        }
    }
}

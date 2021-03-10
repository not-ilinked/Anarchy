using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Discord
{
    public static class MessageExtensions
    {
        #region management
        public static async Task<DiscordMessage> SendMessageAsync(this DiscordClient client, ulong channelId, string message, bool tts = false, DiscordEmbed embed = null)
        {
            return (await client.HttpClient.PostAsync($"/channels/{channelId}/messages", new MessageCreateProperties() { Content = message, Tts = tts, Embed = embed }))
                                 .Deserialize<DiscordMessage>().SetClient(client);
        }

        /// <summary>
        /// Sends a message to a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="message">Contents of the message</param>
        /// <param name="tts">Whether the message should be TTS or not</param>
        /// <returns>The message</returns>
        public static DiscordMessage SendMessage(this DiscordClient client, ulong channelId, string message, bool tts = false, DiscordEmbed embed = null)
        {
            return client.SendMessageAsync(channelId, message, tts, embed).GetAwaiter().GetResult();
        }

        public static async Task<DiscordMessage> SendMessageAsync(this DiscordClient client, ulong channelId, EmbedMaker embed)
        {
            return await client.SendMessageAsync(channelId, null, false, embed);
        }

        public static DiscordMessage SendMessage(this DiscordClient client, ulong channelId, EmbedMaker embed)
        {
            return client.SendMessageAsync(channelId, embed).GetAwaiter().GetResult();
        }


        public static async Task<DiscordMessage> SendFileAsync(this DiscordClient client, ulong channelId, string fileName, byte[] fileData, string message = null, bool tts = false)
        {
            HttpClient httpClient = new HttpClient(new HttpClientHandler() { Proxy = client.Config.Proxy != null && client.Config.Proxy.Type == AnarchyProxyType.HTTP ? new WebProxy(client.Config.Proxy.Host + client.Config.Proxy.Port, false, new string[] { }, new NetworkCredential() { UserName = client.Config.Proxy.Username, Password = client.Config.Proxy.Password }) : null });
            httpClient.DefaultRequestHeaders.Add("Authorization", client.Token);

            MultipartFormDataContent content = new MultipartFormDataContent
            {
                {
                    new StringContent(JsonConvert.SerializeObject(new MessageCreateProperties()
                    {
                        Content = message,
                        Tts = tts
                    })),
                    "payload_json"
                },
                { new ByteArrayContent(fileData), "file", fileName }
            };

            return JsonConvert.DeserializeObject<DiscordMessage>(await httpClient.PostAsync(client.HttpClient.BaseUrl + $"/channels/{channelId}/messages", content)
                                                                            .GetAwaiter().GetResult().Content.ReadAsStringAsync()).SetClient(client);
        }

        /// <summary>
        /// Sends a message with a file attached.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="fileName">Name for the attachment to have</param>
        /// <param name="fileData">Raw byte data from file</param>
        /// <param name="message">Contents of the message</param>
        /// <param name="tts">Whether the message should be TTS or not</param>
        public static DiscordMessage SendFile(this DiscordClient client, ulong channelId, string fileName, byte[] fileData, string message = null, bool tts = false)
        {
            return client.SendFileAsync(channelId, fileName, fileData, message, tts).GetAwaiter().GetResult();
        }


        public static async Task<DiscordMessage> SendFileAsync(this DiscordClient client, ulong channelId, string filePath, string message = null, bool tts = false)
        {
            return await client.SendFileAsync(channelId, new FileInfo(filePath).Name, File.ReadAllBytes(filePath), message, tts);
        }

        /// <summary>
        /// Sends a message with a file attached.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="filePath">Path to the file you want attached</param>
        /// <param name="message">Contents of the message</param>
        /// <param name="tts">Whether the message should be TTS or not</param>
        public static DiscordMessage SendFile(this DiscordClient client, ulong channelId, string filePath, string message = null, bool tts = false)
        {
            return client.SendFileAsync(channelId, filePath, message, tts).GetAwaiter().GetResult();
        }


        public static async Task<DiscordMessage> EditMessageAsync(this DiscordClient client, ulong channelId, ulong messageId, MessageEditProperties properties)
        {
            return (await client.HttpClient.PatchAsync($"/channels/{channelId}/messages/{messageId}", properties))
                                .Deserialize<DiscordMessage>().SetClient(client);
        }

        /// <summary>
        /// Edits a message
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the channel</param>
        /// <param name="message">New content of the message</param>
        /// <returns>The edited message</returns>
        public static DiscordMessage EditMessage(this DiscordClient client, ulong channelId, ulong messageId, MessageEditProperties properties)
        {
            return client.EditMessageAsync(channelId, messageId, properties).GetAwaiter().GetResult();
        }


        public static async Task DeleteMessageAsync(this DiscordClient client, ulong channelId, ulong messageId)
        {
            await client.HttpClient.DeleteAsync($"/channels/{channelId}/messages/{messageId}");
        }

        /// <summary>
        /// Deletes a message
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message</param>
        public static void DeleteMessage(this DiscordClient client, ulong channelId, ulong messageId)
        {
            client.DeleteMessageAsync(channelId, messageId).GetAwaiter().GetResult();
        }


        public static async Task DeleteMessagesAsync(this DiscordClient client, ulong channelId, List<ulong> messages)
        {
            await client.HttpClient.PostAsync($"/channels/{channelId}/messages/bulk-delete", $"{{\"messages\":{JsonConvert.SerializeObject(messages)}}}");
        }

        /// <summary>
        /// Bulk deletes messages (this is a bot only endpoint)
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messages">IDs of the messages</param>
        public static void DeleteMessages(this DiscordClient client, ulong channelId, List<ulong> messages)
        {
            client.DeleteMessagesAsync(channelId, messages).GetAwaiter().GetResult();
        }
        #endregion


        public static async Task TriggerTypingAsync(this DiscordClient client, ulong channelId)
        {
            var resp = await client.HttpClient.PostAsync($"/channels/{channelId}/typing");

            if (resp.ToString().Contains("cooldown"))
                throw new RateLimitException(client, resp.Deserialize<JObject>().GetValue("message_send_cooldown_ms").ToObject<int>());
        }

        /// <summary>
        /// Triggers a 'user typing...'
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        public static void TriggerTyping(this DiscordClient client, ulong channelId)
        {
            client.TriggerTypingAsync(channelId).GetAwaiter().GetResult();
        }


        /// <summary>
        /// Gets a list of messages from a channel.
        /// The list is ordered first -> last.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="filters">Options for filtering out messages</param>
        public static async Task<IReadOnlyList<DiscordMessage>> GetChannelMessagesAsync(this DiscordClient client, ulong channelId, MessageFilters filters = null)
        {
            if (filters == null)
                filters = new MessageFilters();

            const int messagesPerRequest = 100;

            List<DiscordMessage> messages = new List<DiscordMessage>();

            while (true)
            {
                string parameters = "";
                if (filters.Limit.HasValue)
                    parameters += $"limit={(uint)Math.Min(messagesPerRequest, filters.Limit.Value - messages.Count)}&";
                else
                    parameters += $"limit={messagesPerRequest}&";
                if (filters.BeforeId.HasValue)
                    parameters += $"before={filters.BeforeId.Value}&";
                if (filters.AfterId.HasValue)
                    parameters += $"after={filters.AfterId.Value}&";
                
                var newMessages = (await client.HttpClient.GetAsync($"/channels/{channelId}/messages?{parameters}"))
                                                          .Deserialize<IReadOnlyList<DiscordMessage>>().SetClientsInList(client);

                messages.AddRange(newMessages);

                filters.BeforeId = messages.Last().Id;

                if (newMessages.Count < messagesPerRequest)
                    break;
            }

            return messages;
        }

        public static IReadOnlyList<DiscordMessage> GetChannelMessages(this DiscordClient client, ulong channelId, MessageFilters filters = null)
        {
            return client.GetChannelMessagesAsync(channelId, filters).GetAwaiter().GetResult();
        }

        
        /// <summary>
        /// Gets a list of messages from a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="limit">Max amount of messages to receive</param>
        public static async Task<IReadOnlyList<DiscordMessage>> GetChannelMessagesAsync(this DiscordClient client, ulong channelId, uint limit)
        {
            return await client.GetChannelMessagesAsync(channelId, new MessageFilters() { Limit = limit });
        }

        public static IReadOnlyList<DiscordMessage> GetChannelMessages(this DiscordClient client, ulong channelId, uint limit)
        {
            return client.GetChannelMessagesAsync(channelId, limit).GetAwaiter().GetResult();
        }


        public static async Task<IReadOnlyList<DiscordUser>> GetMessageReactionsAsync(this DiscordClient client, ulong channelId, ulong messageId, ReactionQuery query)
        {
            return (await client.HttpClient.GetAsync($"/channels/{channelId}/messages/{messageId}/reactions/{query.ReactionName}{(query.ReactionId.HasValue ? ":" + query.ReactionId.Value : "")}?limit={query.Limit}&after={query.AfterId}"))
                                .Deserialize<IReadOnlyList<DiscordUser>>().SetClientsInList(client);
        }

        /// <summary>
        /// Gets a message's reactions
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message</param>
        /// <param name="reaction">The reaction</param>
        /// <param name="limit">Max amount of reactions to receive</param>
        /// <param name="afterId">Reaction ID to offset from</param>
        public static IReadOnlyList<DiscordUser> GetMessageReactions(this DiscordClient client, ulong channelId, ulong messageId, ReactionQuery query)
        {
            return client.GetMessageReactionsAsync(channelId, messageId, query).GetAwaiter().GetResult();
        }


        public static async Task AddMessageReactionAsync(this DiscordClient client, ulong channelId, ulong messageId, string reactionName, ulong? reactionId = null)
        {
            await client.HttpClient.PutAsync($"/channels/{channelId}/messages/{messageId}/reactions/{reactionName}{(reactionId.HasValue ? ":" + reactionId.Value : "")}/@me");
        }

        /// <summary>
        /// Adds a reaction to a message
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message</param>
        /// <param name="reaction">The reaction to add</param>
        public static void AddMessageReaction(this DiscordClient client, ulong channelId, ulong messageId, string reactionName, ulong? reactionId = null)
        {
            client.AddMessageReactionAsync(channelId, messageId, reactionName, reactionId).GetAwaiter().GetResult();
        }


        private static async Task removeReactionAsync(this DiscordClient client, ulong channelId, ulong messageId, string user, string reactionName, ulong? reactionId = null)
        {
            await client.HttpClient.DeleteAsync($"/channels/{channelId}/messages/{messageId}/reactions/{reactionName}{(reactionId.HasValue ? ":" + reactionId.Value : "")}/{user}");
        }


        public static async Task RemoveMessageReactionAsync(this DiscordClient client, ulong channelId, ulong messageId, string reactionName, ulong? reactionId = null)
        {
            await client.removeReactionAsync(channelId, messageId, "@me", reactionName, reactionId);
        }

        public static void RemoveMessageReaction(this DiscordClient client, ulong channelId, ulong messageId, string reactionName, ulong? reactionId)
        {
            client.RemoveMessageReactionAsync(channelId, messageId, reactionName, reactionId).GetAwaiter().GetResult();
        }


        public static async Task RemoveMessageReactionAsync(this DiscordClient client, ulong channelId, ulong messageId, ulong userId, string reactionName, ulong? reactionId = null)
        {
            await client.removeReactionAsync(channelId, messageId, userId.ToString(), reactionName, reactionId);
        }

        public static void RemoveMessageReaction(this DiscordClient client, ulong channelId, ulong messageId, ulong userId, string reactionName, ulong? reactionId = null)
        {
            client.RemoveMessageReactionAsync(channelId, messageId, userId, reactionName, reactionId).GetAwaiter().GetResult();
        }


        #region pins
        [Obsolete("GetChannelPinnedMessagesAsync is depricated. Call GetPinnedMessagesAsync instead", true)]
        public static Task<IReadOnlyList<DiscordMessage>> GetChannelPinnedMessagesAsync(this DiscordClient client, ulong channelId)
        {
            return null;
        }

        public static async Task<IReadOnlyList<DiscordMessage>> GetPinnedMessagesAsync(this DiscordClient client, ulong channelId)
        {
            return (await client.HttpClient.GetAsync($"/channels/{channelId}/pins"))
                                .Deserialize<IReadOnlyList<DiscordMessage>>().SetClientsInList(client);
        }

        [Obsolete("GetChannelPinnedMessages is depricated. Call GetPinnedMessages instead", true)]
        public static IReadOnlyList<DiscordMessage> GetChannelPinnedMessages(this DiscordClient client, ulong channelId)
        {
            return null;
        }

        /// <summary>
        /// Gets a channel's pinned messages
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        public static IReadOnlyList<DiscordMessage> GetPinnedMessages(this DiscordClient client, ulong channelId)
        {
            return client.GetPinnedMessagesAsync(channelId).GetAwaiter().GetResult();
        }


        [Obsolete("PinChannelMessageAsync is depricated. Call PinMessageAsync instead", true)]
        public static Task PinChannelMessageAsync(this DiscordClient client, ulong channelId, ulong messageId)
        {
            return null;
        }

        public static async Task PinMessageAsync(this DiscordClient client, ulong channelId, ulong messageId)
        {
            await client.HttpClient.PutAsync($"/channels/{channelId}/pins/{messageId}");
        }

        [Obsolete("PinChannelMessage is depricated. Call PinMessage instead", true)]
        public static void PinChannelMessage(this DiscordClient client, ulong channelId, ulong messageId)
        {

        }

        /// <summary>
        /// Pins a message to a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message</param>
        public static void PinMessage(this DiscordClient client, ulong channelId, ulong messageId)
        {
            client.PinMessageAsync(channelId, messageId).GetAwaiter().GetResult();
        }


        public static async Task UnpinChannelMessageAsync(this DiscordClient client, ulong channelId, ulong messageId)
        {
            await client.HttpClient.DeleteAsync($"/channels/{channelId}/pins/{messageId}");
        }

        /// <summary>
        /// Unpins a message from a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message</param>
        public static void UnpinChannelMessage(this DiscordClient client, ulong channelId, ulong messageId)
        {
            client.UnpinChannelMessageAsync(channelId, messageId).GetAwaiter().GetResult();
        }
        #endregion


        public static async Task<DiscordMessage> CrosspostMessageAsync(this DiscordClient client, ulong channelId, ulong messageId)
        {
            return (await client.HttpClient.PostAsync($"/channels/{channelId}/messages/{messageId}/crosspost")).Deserialize<DiscordMessage>().SetClient(client);
        }

        public static DiscordMessage CrosspostMessage(this DiscordClient client, ulong channelId, ulong messageId)
        {
            return client.CrosspostMessageAsync(channelId, messageId).GetAwaiter().GetResult();
        }


        public static async Task AcknowledgeMessageAsync(this DiscordClient client, ulong channelId, ulong messageId)
        {
            await client.HttpClient.PostAsync($"/channels/{channelId}/messages/{messageId}/ack", "{\"token\":null}");
        }

        public static void AcknowledgeMessage(this DiscordClient client, ulong channelId, ulong messageId)
        {
            client.AcknowledgeMessageAsync(channelId, messageId).GetAwaiter().GetResult();
        }
    }
}
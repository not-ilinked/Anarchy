using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Discord
{
    public static class MessageExtensions
    {
        #region management
        /// <summary>
        /// Sends a message to a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="message">Contents of the message</param>
        /// <param name="tts">Whether the message should be TTS or not</param>
        /// <returns>The message</returns>
        public static DiscordMessage SendMessage(this DiscordClient client, ulong channelId, string message, bool tts = false, DiscordEmbed embed = null)
        {
            return client.HttpClient.Post($"/channels/{channelId}/messages", new MessageProperties() { Content = message, Tts = tts, Embed = embed })
                                 .Deserialize<DiscordMessage>().SetClient(client);
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
            HttpClient httpClient = new HttpClient(new HttpClientHandler() { Proxy = client.Config.Proxy != null && client.Config.Proxy.Type == Leaf.xNet.ProxyType.HTTP ? new WebProxy(client.Config.Proxy.Host + client.Config.Proxy.Port, false, new string[] { }, new NetworkCredential() { UserName = client.Config.Proxy.Username, Password = client.Config.Proxy.Password }) : null });
            httpClient.DefaultRequestHeaders.Add("Authorization", client.Token);

            MultipartFormDataContent content = new MultipartFormDataContent
            {
                {
                    new StringContent(JsonConvert.SerializeObject(new MessageProperties()
                    {
                        Content = message,
                        Tts = tts
                    })),
                    "payload_json"
                },
                { new ByteArrayContent(fileData), "file", fileName }
            };

            return httpClient.PostAsync(client.Config.ApiBaseUrl + $"/channels/{channelId}/messages", content).Result
                                    .Content.ReadAsStringAsync().Result.Deserialize<DiscordMessage>().SetClient(client);
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
            return client.SendFile(channelId, new FileInfo(filePath).Name, File.ReadAllBytes(filePath), message, tts);
        }


        /// <summary>
        /// Edits a message
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the channel</param>
        /// <param name="message">New content of the message</param>
        /// <returns>The edited message</returns>
        public static DiscordMessage EditMessage(this DiscordClient client, ulong channelId, ulong messageId, string message)
        {
            return client.HttpClient.Patch($"/channels/{channelId}/messages/{messageId}", $"{{\"content\":\"{message}\"}}")
                                .Deserialize<DiscordMessage>().SetClient(client);
        }


        /// <summary>
        /// Deletes a message
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message</param>
        public static void DeleteMessage(this DiscordClient client, ulong channelId, ulong messageId)
        {
            client.HttpClient.Delete($"/channels/{channelId}/messages/{messageId}");
        }


        /// <summary>
        /// Bulk deletes messages (this is a bot only endpoint)
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messages">IDs of the messages</param>
        public static void DeleteMessages(this DiscordClient client, ulong channelId, List<ulong> messages)
        {
            client.HttpClient.Post($"/channels/{channelId}/messages/bulk-delete", $"{{\"messages\":{JsonConvert.SerializeObject(messages)}}}");
        }
        #endregion


        /// <summary>
        /// Triggers a 'user typing...'
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        public static void TriggerTyping(this DiscordClient client, ulong channelId)
        {
            var resp = client.HttpClient.Post($"/channels/{channelId}/typing");

            if (resp.ToString().Contains("cooldown"))
                throw new RateLimitException(client, resp.Deserialize<JObject>().GetValue("message_send_cooldown_ms").ToObject<int>());
        }


        /// <summary>
        /// Gets a list of messages from a channel.
        /// The list is ordered first -> last.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="filters">Options for filtering out messages</param>
        public static IReadOnlyList<DiscordMessage> GetChannelMessages(this DiscordClient client, ulong channelId, MessageFilters filters = null)
        {
            if (filters == null)
                filters = new MessageFilters();

            const int messagesPerRequest = 100;

            List<DiscordMessage> messages = new List<DiscordMessage>();

            while (true)
            {
                string parameters = "";
                if (filters.Limit.HasValue)
                    parameters += $"limit={(uint)Math.Min(messagesPerRequest, filters.Limit.Value - messages.Count)}";
                if (filters.BeforeId.HasValue)
                    parameters += $"before={filters.BeforeId.Value}&";
                if (filters.AfterId.HasValue)
                    parameters += $"after={filters.AfterId.Value}&";
                
                var newMessages = client.HttpClient.Get($"/channels/{channelId}/messages?{parameters}")
                                                          .Deserialize<IReadOnlyList<DiscordMessage>>().SetClientsInList(client);

                messages.AddRange(newMessages);

                filters.BeforeId = messages.Last().Id;

                if (newMessages.Count < messagesPerRequest)
                    break;
            }

            return messages;
        }

        
        /// <summary>
        /// Gets a list of messages from a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="limit">Max amount of messages to receive</param>
        public static IReadOnlyList<DiscordMessage> GetChannelMessages(this DiscordClient client, ulong channelId, uint limit)
        {
            return client.GetChannelMessages(channelId, new MessageFilters() { Limit = limit });
        }

        
        /// <summary>
        /// Gets a message's reactions
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message</param>
        /// <param name="reaction">The reaction</param>
        /// <param name="limit">Max amount of reactions to receive</param>
        /// <param name="afterId">Reaction ID to offset from</param>
        public static IReadOnlyList<DiscordUser> GetMessageReactions(this DiscordClient client, ulong channelId, ulong messageId, string reaction, uint limit = 25, ulong afterId = 0)
        {
            return client.HttpClient.Get($"/channels/{channelId}/messages/{messageId}/reactions/{reaction}?limit={limit}&after={afterId}")
                                .Deserialize<IReadOnlyList<DiscordUser>>().SetClientsInList(client);
        }


        #region pins
        /// <summary>
        /// Gets a channel's pinned messages
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        public static IReadOnlyList<DiscordMessage> GetChannelPinnedMessages(this DiscordClient client, ulong channelId)
        {
            return client.HttpClient.Get($"/channels/{channelId}/pins")
                                .Deserialize<IReadOnlyList<DiscordMessage>>().SetClientsInList(client);
        }


        /// <summary>
        /// Pins a message to a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message</param>
        public static void PinChannelMessage(this DiscordClient client, ulong channelId, ulong messageId)
        {
            client.HttpClient.Put($"/channels/{channelId}/pins/{messageId}");
        }


        /// <summary>
        /// Unpins a message from a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message</param>
        public static void UnpinChannelMessage(this DiscordClient client, ulong channelId, ulong messageId)
        {
            client.HttpClient.Delete($"/channels/{channelId}/pins/{messageId}");
        }
        #endregion


        public static void AcknowledgeMessage(this DiscordClient client, ulong channelId, ulong messageId)
        {
            client.HttpClient.Post($"/channels/{channelId}/messages/{messageId}/ack", "{\"token\":null}");
        }
    }
}
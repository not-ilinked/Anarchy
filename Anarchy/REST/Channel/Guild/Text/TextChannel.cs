using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Represents a <see cref="DiscordChannel"/> specific to guild text channels
    /// </summary>
    public class TextChannel : GuildChannel, IMessageChannel
    {
        [JsonProperty("topic")]
        public string Topic { get; private set; }

        [JsonProperty("nsfw")]
        public bool Nsfw { get; private set; }

        [JsonProperty("last_message_id")]
        public ulong? LastMessageId { get; private set; }

        private int _slowMode;
        [JsonProperty("rate_limit_per_user")]
        public int SlowMode
        {
            get { return _slowMode; }
            private set { _slowMode = value * 10; } //convert to milliseconds
        }

        protected void Update(TextChannel channel)
        {
            base.Update(channel);
            Type = channel.Type;
            Topic = channel.Topic;
            Nsfw = channel.Nsfw;
            SlowMode = channel.SlowMode;
        }

        public new async Task UpdateAsync()
        {
            Update((TextChannel) await Client.GetChannelAsync(Id));
        }

        /// <summary>
        /// Updates the channel's info
        /// </summary>
        public new void Update()
        {
            UpdateAsync().GetAwaiter().GetResult();
        }

        public async Task ModifyAsync(TextChannelProperties properties)
        {
            Update(await Client.ModifyGuildChannelAsync(Id, properties));

            if (properties.TypeProperty.Set)
                Type = properties.TypeProperty;
        }

        /// <summary>
        /// Modifies the channel
        /// </summary>
        /// <param name="properties">Options for modifying the channel</param>
        public void Modify(TextChannelProperties properties)
        {
            ModifyAsync(properties).GetAwaiter().GetResult();
        }

        #region messages
        public async Task TriggerTypingAsync()
        {
            await Client.TriggerTypingAsync(Id);
        }

        /// <summary>
        /// Triggers a 'user typing...'
        /// </summary>
        public void TriggerTyping()
        {
            TriggerTypingAsync().GetAwaiter().GetResult();
        }

        public Task<DiscordMessage> SendMessageAsync(MessageProperties properties)
        {
            return Client.SendMessageAsync(Id, properties);
        }

        public DiscordMessage SendMessage(MessageProperties properties)
        {
            return SendMessageAsync(properties).GetAwaiter().GetResult();
        }

        public async Task<DiscordMessage> SendMessageAsync(string message, bool tts = false, DiscordEmbed embed = null)
        {
            return await Client.SendMessageAsync(Id, message, tts, embed);
        }

        /// <summary>
        /// Sends a message to the channel
        /// </summary>
        /// <param name="message">Content of the message</param>
        /// <param name="tts">Whether the message should be TTS or not</param>
        /// <returns>The message</returns>
        public DiscordMessage SendMessage(string message, bool tts = false, DiscordEmbed embed = null)
        {
            return SendMessageAsync(message, tts, embed).Result;
        }

        public Task<DiscordMessage> SendMessageAsync(EmbedMaker embed)
        {
            return Client.SendMessageAsync(Id, embed);
        }

        public DiscordMessage SendMessage(EmbedMaker embed)
        {
            return SendMessageAsync(embed).GetAwaiter().GetResult();
        }

        public async Task<DiscordMessage> SendFileAsync(string fileName, byte[] fileData, string message = null, bool tts = false)
        {
            return await Client.SendFileAsync(Id, fileName, fileData, message, tts);
        }

        public DiscordMessage SendFile(string fileName, byte[] fileData, string message = null, bool tts = false)
        {
            return SendFileAsync(fileName, fileData, message, tts).Result;
        }

        public async Task<DiscordMessage> SendFileAsync(string filePath, string message = null, bool tts = false)
        {
            return await Client.SendFileAsync(Id, filePath, message, tts);
        }

        public DiscordMessage SendFile(string filePath, string message = null, bool tts = false)
        {
            return SendFileAsync(filePath, message, tts).Result;
        }

        public async Task DeleteMessagesAsync(List<ulong> messages)
        {
            await Client.DeleteMessagesAsync(Id, messages);
        }

        /// <summary>
        /// Bulk deletes messages (this is a bot only endpoint)
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messages">IDs of the messages</param>
        public void DeleteMessages(List<ulong> messages)
        {
            DeleteMessagesAsync(messages).GetAwaiter().GetResult();
        }

        public async Task<IReadOnlyList<DiscordMessage>> GetMessagesAsync(MessageFilters filters = null)
        {
            return await Client.GetChannelMessagesAsync(Id, filters);
        }

        /// <summary>
        /// Gets a list of messages from the channel
        /// </summary>
        /// <param name="filters">Options for filtering out messages</param>
        public IReadOnlyList<DiscordMessage> GetMessages(MessageFilters filters = null)
        {
            return GetMessagesAsync(filters).GetAwaiter().GetResult();
        }

        public async Task<IReadOnlyList<DiscordMessage>> GetPinnedMessagesAsync()
        {
            return await Client.GetPinnedMessagesAsync(Id);
        }

        /// <summary>
        /// Gets the channel's pinned messages
        /// </summary>
        public IReadOnlyList<DiscordMessage> GetPinnedMessages()
        {
            return GetPinnedMessagesAsync().Result;
        }

        public async Task PinMessageAsync(ulong messageId)
        {
            await Client.PinMessageAsync(Id, messageId);
        }

        /// <summary>
        /// Pins a message to the channel
        /// </summary>
        /// <param name="messageId">ID of the message</param>
        public void PinMessage(ulong messageId)
        {
            PinMessageAsync(messageId).GetAwaiter().GetResult();
        }

        public async Task UnpinMessageAsync(ulong messageId)
        {
            await Client.UnpinChannelMessageAsync(Id, messageId);
        }

        /// <summary>
        /// Unpins a message from the channel
        /// </summary>
        /// <param name="messageId">ID of the message</param>
        public void UnpinMessage(ulong messageId)
        {
            UnpinMessageAsync(messageId).GetAwaiter().GetResult();
        }
        #endregion

        public async Task<ulong> FollowAsync(ulong crosspostChannelId)
        {
            return await Client.FollowChannelAsync(Id, crosspostChannelId);
        }

        public ulong Follow(ulong crosspostChannelId)
        {
            if (Type != ChannelType.News)
                throw new InvalidOperationException("Channel must be of type News");

            return FollowAsync(crosspostChannelId).GetAwaiter().GetResult();
        }

        public async Task<DiscordInvite> CreateInviteAsync(InviteProperties properties = null)
        {
            return await Client.CreateInviteAsync(Id, properties);
        }

        /// <summary>
        /// Creates an invite for the channel
        /// </summary>
        /// <param name="properties">Options for creating the invite</param>
        /// <returns>The created invite</returns>
        public DiscordInvite CreateInvite(InviteProperties properties = null)
        {
            return CreateInviteAsync(properties).GetAwaiter().GetResult();
        }

        public async Task<IReadOnlyList<DiscordWebhook>> GetWebhooksAsync()
        {
            return await Client.GetChannelWebhooksAsync(Id);
        }

        /// <summary>
        /// Gets the channel's webhooks
        /// </summary>
        public IReadOnlyList<DiscordWebhook> GetWebhooks()
        {
            return GetWebhooksAsync().GetAwaiter().GetResult();
        }

        public async Task<DiscordDefaultWebhook> CreateWebhookAsync(DiscordWebhookProperties properties)
        {
            return await Client.CreateWebhookAsync(Id, properties);
        }

        /// <summary>
        /// Creates a webhook
        /// </summary>
        /// <param name="properties">Options for creating/modifying the webhook</param>
        /// <returns>The created webhook</returns>
        public DiscordDefaultWebhook CreateWebhook(DiscordWebhookProperties properties)
        {
            return CreateWebhookAsync(properties).Result;
        }

        public Task<DiscordThread> CreateThreadAsync(ulong messageId, string name, TimeSpan ttl)
            => Client.CreateThreadAsync(Id, messageId, name, ttl);

        public DiscordThread CreateThread(ulong messageId, string name, TimeSpan ttl)
            => CreateThreadAsync(messageId, name, ttl).GetAwaiter().GetResult();

        public Task<DiscordThread> CreateThreadAsync(string name, TimeSpan ttl)
            => Client.CreateThreadAsync(Id, name, ttl);

        public DiscordThread CreateThread(string name, TimeSpan ttl)
            => CreateThreadAsync(name, ttl).GetAwaiter().GetResult();

        internal override void SetLastMessageId(ulong id)
        {
            LastMessageId = id;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Discord
{
    public class DiscordThread : DiscordChannel
    {
        [JsonPropertyName("guild_id")]
        private readonly ulong _guildId;
        public MinimalGuild Guild => new MinimalGuild(_guildId).SetClient(Client);

        [JsonPropertyName("parent_id")]
        private readonly ulong _parentId;
        public MinimalTextChannel Channel => new MinimalTextChannel(_parentId).SetClient(Client);

        [JsonPropertyName("owner_id")]
        public ulong OwnerId { get; private set; }

        [JsonPropertyName("last_message_id")]
        public ulong? LastMessageId { get; private set; }

        private int _slowMode;
        [JsonPropertyName("rate_limit_per_user")]
        public int SlowMode
        {
            get { return _slowMode; }
            private set { _slowMode = value * 10; } //convert to milliseconds
        }

        [JsonPropertyName("member_count")]
        public int MemberCount { get; internal set; }

        [JsonPropertyName("flags")]
        public int Flags { get; private set; }

        [JsonPropertyName("thread_members_preview")]
        public IReadOnlyList<ulong> MemberPreview { get; internal set; }

        [JsonPropertyName("thread_metadata")]
        public DiscordThreadMetadata Metadata { get; private set; }

        protected void Update(DiscordThread thread)
        {
            base.Update(thread);
            LastMessageId = thread.LastMessageId;
            _slowMode = thread._slowMode;
            MemberCount = thread.MemberCount;
            Flags = thread.Flags;
            MemberPreview = thread.MemberPreview;
            Metadata = thread.Metadata;
        }

        public static new Task UpdateAsync()
        {
            throw new InvalidOperationException("Object is unfit for UpdateAsync");
        }

        public static new void Update()
        {
            UpdateAsync().GetAwaiter().GetResult();
        }

        public async Task ModifyAsync(ThreadProperties changes)
        {
            Update(await Client.ModifyThreadAsync(Id, changes));
        }

        public void Modify(ThreadProperties changes)
        {
            ModifyAsync(changes).GetAwaiter().GetResult();
        }

        public Task JoinAsync()
            => Client.JoinThreadAsync(Id);

        public void Join()
            => JoinAsync().GetAwaiter().GetResult();

        public Task LeaveAsync()
            => Client.LeaveThreadAsync(Id);

        public void Leave()
            => LeaveAsync().GetAwaiter().GetResult();

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
    }
}

using Discord.Webhook;
using Newtonsoft.Json;
using System.Collections.Generic;

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
            Topic = channel.Topic;
            Nsfw = channel.Nsfw;
            SlowMode = channel.SlowMode;
        }


        /// <summary>
        /// Updates the channel's info
        /// </summary>
        public new void Update()
        {
            Update(Client.GetChannel(Id).ToTextChannel());
        }


        /// <summary>
        /// Modifies the channel
        /// </summary>
        /// <param name="properties">Options for modifying the channel</param>
        public void Modify(TextChannelProperties properties)
        {
            Update(Client.ModifyGuildChannel(Id, properties));
        }


        #region messages
        /// <summary>
        /// Triggers a 'user typing...'
        /// </summary>
        public void TriggerTyping()
        {
            Client.TriggerTyping(Id);
        }


        /// <summary>
        /// Sends a message to the channel
        /// </summary>
        /// <param name="message">Content of the message</param>
        /// <param name="tts">Whether the message should be TTS or not</param>
        /// <returns>The message</returns>
        public DiscordMessage SendMessage(string message, bool tts = false, DiscordEmbed embed = null)
        {
            return Client.SendMessage(Id, message, tts, embed);
        }


        public DiscordMessage SendFile(string fileName, byte[] fileData, string message = null, bool tts = false)
        {
            return Client.SendFile(Id, fileName, fileData, message, tts);
        }


        public DiscordMessage SendFile(string filePath, string message = null, bool tts = false)
        {
            return Client.SendFile(Id, filePath, message, tts);
        }


        /// <summary>
        /// Gets a list of messages from the channel
        /// </summary>
        /// <param name="filters">Options for filtering out messages</param>
        public IReadOnlyList<DiscordMessage> GetMessages(MessageFilters filters = null)
        {
            return Client.GetChannelMessages(Id, filters);
        }


        /// <summary>
        /// Bulk deletes messages (this is a bot only endpoint)
        /// </summary>
        /// <param name="messages">IDs of the messages</param>
        public void DeleteMessages(List<ulong> messages)
        {
            Client.DeleteMessages(Id, messages);
        }


        /// <summary>
        /// Gets the channel's pinned messages
        /// </summary>
        public IReadOnlyList<DiscordMessage> GetPinnedMessages()
        {
            return Client.GetChannelPinnedMessages(Id);
        }


        /// <summary>
        /// Pins a message to the channel
        /// </summary>
        /// <param name="messageId">ID of the message</param>
        public void PinMessage(ulong messageId)
        {
            Client.PinChannelMessage(Id, messageId);
        }


        /// <summary>
        /// Pins a message to the channel
        /// </summary>
        public void PinMessage(DiscordMessage message)
        {
            PinMessage(message.Id);
        }


        /// <summary>
        /// Unpins a message from the channel
        /// </summary>
        /// <param name="messageId">ID of the message</param>
        public void UnpinMessage(ulong messageId)
        {
            Client.UnpinChannelMessage(Id, messageId);
        }


        /// <summary>
        /// Unpins a message from the channel
        /// </summary>
        public void UnpinMessage(DiscordMessage message)
        {
            Client.UnpinChannelMessage(Id, message.Id);
        }
        #endregion


        /// <summary>
        /// Creates an invite for the channel
        /// </summary>
        /// <param name="properties">Options for creating the invite</param>
        /// <returns>The created invite</returns>
        public DiscordInvite CreateInvite(InviteProperties properties = null)
        {
            return Client.CreateInvite(Id, properties);
        }


        /// <summary>
        /// Gets the channel's webhooks
        /// </summary>
        public IReadOnlyList<DiscordWebhook> GetWebhooks()
        {
            return Client.GetChannelWebhooks(Id);
        }


        /// <summary>
        /// Creates a webhook
        /// </summary>
        /// <param name="properties">Options for creating/modifying the webhook</param>
        /// <returns>The created webhook</returns>
        public DiscordWebhook CreateWebhook(DiscordWebhookProperties properties)
        {
            properties.ChannelId = Id;

            return Client.CreateChannelWebhook(Id, properties);
        }
    }
}

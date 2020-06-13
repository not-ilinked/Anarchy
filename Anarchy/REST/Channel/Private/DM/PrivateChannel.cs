using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Represents a <see cref="DiscordChannel"/> specific to direct message channels
    /// </summary>
    public class PrivateChannel : DiscordChannel, IMessageChannel
    {
        public PrivateChannel()
        {
            OnClientUpdated += (sender, e) => Recipients.SetClientsInList(Client);
        }

        [JsonProperty("recipients")]
        internal List<DiscordUser> _recipients;


        [JsonProperty("last_message_id")]
        public ulong? LastMessageId { get; internal set; }


        public IReadOnlyList<DiscordUser> Recipients
        {
            get 
            {
                if (_recipients == null)
                    return new List<DiscordUser>();
                else
                    return _recipients;
            }
        }


        protected void Update(PrivateChannel channel)
        {
            base.Update(channel);
            _recipients = channel._recipients;
        }


        /// <summary>
        /// Updates the channel's info
        /// </summary>
        public new void Update()
        {
            Update(Client.GetChannel(Id).ToDMChannel());
        }


        /// <summary>
        /// Closes the DM
        /// </summary>
        public void Leave()
        {
            Update(Client.DeleteChannel(Id).ToDMChannel());
        }


        public void ChangeCallRegion(string regionId)
        {
            Client.ChangePrivateCallRegion(Id, regionId);
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
        /// Bulk deletes messages (this is a bot only endpoint)
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messages">IDs of the messages</param>
        public void DeleteMessages(List<ulong> messages)
        {
            Client.DeleteMessages(Id, messages);
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
    }
}

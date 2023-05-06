﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Discord
{
    public class DiscordMessage : Controllable
    {
        public DiscordMessage()
        {
            Reactions = new List<MessageReaction>();

            OnClientUpdated += (sender, e) =>
            {
                Reactions.SetClientsInList(Client);
                Mentions.SetClientsInList(Client);

                _authorUser.SetClient(Client);

                if (_authorMember != null)
                {
                    _authorMember.SetClient(Client);

                    if (Guild != null)
                        _authorMember.GuildId = Guild.Id;
                }

                if (_authorUser == null) // when the client itself is the author, "user" doesn't get sent
                    _authorUser = Client.User;
            };
        }

        [JsonPropertyName("id")]
        public ulong Id { get; private set; }

        [JsonPropertyName("content")]
        public string Content { get; private set; }

        [JsonPropertyName("tts")]
        public bool Tts { get; private set; }

        [JsonPropertyName("author")]
        private DiscordUser _authorUser;

        [JsonPropertyName("member")]
        private GuildMember _authorMember;

        public MessageAuthor Author
        {
            get
            {
                if (_authorMember != null)
                    _authorMember.User = _authorUser;

                return new MessageAuthor(_authorUser, _authorMember);
            }
        }

        [JsonPropertyName("attachments")]
        public IReadOnlyList<DiscordAttachment> Attachments { get; private set; }

        [JsonPropertyName("embeds")]
        private IReadOnlyList<DiscordEmbed> _embeds;
        public DiscordEmbed Embed
        {
            get { return _embeds == null || _embeds.Count == 0 ? null : _embeds[0]; }
            private set { _embeds = new List<DiscordEmbed>() { value }; }
        }

        [JsonPropertyName("reactions")]
        public IReadOnlyList<MessageReaction> Reactions { get; private set; }

        [JsonPropertyName("mentions")]
        public IReadOnlyList<DiscordUser> Mentions { get; private set; }

        [JsonPropertyName("mention_roles")]
        public IReadOnlyList<ulong> MentionedRoles { get; private set; }

        [JsonPropertyName("mention_everyone")]
        public bool MentionedEveryone { get; private set; }

        [JsonPropertyName("timestamp")]
        public DateTime SentAt { get; private set; }

        [JsonPropertyName("edited_timestamp")]
        public DateTime? EditedAt { get; private set; }

        [JsonPropertyName("pinned")]
        public bool Pinned { get; private set; }

        [JsonPropertyName("channel_id")]
        private readonly ulong _channelId;

        public MinimalTextChannel Channel => new MinimalTextChannel(_channelId).SetClient(Client);

        [JsonPropertyName("guild_id")]
        internal ulong? GuildId { get; set; }

        /// <summary>
        /// This is sometimes null, even when it has been sent through a guild
        /// </summary>
        public MinimalGuild Guild
        {
            get
            {
                if (GuildId.HasValue)
                    return new MinimalGuild(GuildId.Value).SetClient(Client);
                else
                    return null;
            }
        }

        [JsonPropertyName("type")]
        public MessageType Type { get; private set; }

        [JsonPropertyName("flags")]
        public MessageFlags Flags { get; private set; }

        [JsonPropertyName("referenced_message")]
        public DiscordMessage ReferencedMessage { get; private set; }

        [JsonPropertyName("message_reference")]
        public MessageReference MessageReference { get; private set; }

        [JsonPropertyName("components")]
        [JsonConverter(typeof(DeepJsonConverter<MessageComponent>))]
        public List<MessageComponent> Components { get; private set; }

        [JsonPropertyName("sticker_items")]
        public IReadOnlyList<DiscordSticker> Stickers { get; private set; }

        private void Update(DiscordMessage updated)
        {
            Content = updated.Content;
            Pinned = updated.Pinned;
            Mentions = updated.Mentions;
            MentionedRoles = updated.MentionedRoles;
            MentionedEveryone = updated.MentionedEveryone;
            Embed = updated.Embed;
            Flags = updated.Flags;
            ReferencedMessage = updated.ReferencedMessage;
            MessageReference = updated.MessageReference;
            EditedAt = updated.EditedAt;
            Reactions = updated.Reactions;
            Stickers = updated.Stickers;
            _authorMember = updated.Author.Member;
            _authorUser = updated.Author.User;
        }

        public async Task EditAsync(MessageEditProperties properties)
        {
            if (Type != MessageType.Default)
                throw new InvalidOperationException("Can only edit messages of type Default");

            Update(await Client.EditMessageAsync(Channel.Id, Id, properties));
        }

        /// <summary>
        /// Edits the message
        /// </summary>
        /// <param name="message">The new contents of the message</param>
        public void Edit(MessageEditProperties properties)
        {
            EditAsync(properties).GetAwaiter().GetResult();
        }

        public async Task DeleteAsync()
        {
            await Client.DeleteMessageAsync(Channel.Id, Id);
        }

        /// <summary>
        /// Deletes the message
        /// </summary>
        public void Delete()
        {
            DeleteAsync().GetAwaiter().GetResult();
        }

        public async Task AcknowledgeAsync()
        {
            await Client.AcknowledgeMessageAsync(Channel.Id, Id);
        }

        public void Acknowledge()
        {
            AcknowledgeAsync().GetAwaiter().GetResult();
        }

        public async Task<IReadOnlyList<DiscordUser>> GetReactionsAsync(ReactionQuery query)
        {
            return await Client.GetMessageReactionsAsync(Channel.Id, Id, query);
        }

        /// <summary>
        /// Gets instances of a reaction to a message
        /// </summary>
        /// <param name="reaction">The reaction</param>
        /// <param name="limit">Max amount of reactions to receive</param>
        /// <param name="afterId">The reaction ID to offset from</param>
        public IReadOnlyList<DiscordUser> GetReactions(ReactionQuery query)
        {
            return GetReactionsAsync(query).GetAwaiter().GetResult();
        }

        public async Task AddReactionAsync(string reactionName, ulong? reactionId = null)
        {
            await Client.AddMessageReactionAsync(Channel.Id, Id, reactionName, reactionId);
        }

        /// <summary>
        /// Adds a reaction to the message
        /// </summary>
        public void AddReaction(string reactionName, ulong? reactionId = null)
        {
            AddReactionAsync(reactionName, reactionId).GetAwaiter().GetResult();
        }

        public async Task RemoveClientReactionAsync(string reactionName, ulong? reactionId = null)
        {
            await Client.RemoveMessageReactionAsync(Channel.Id, Id, reactionName, reactionId);
        }

        public void RemoveClientReaction(string reactionName, ulong? reactionId = null)
        {
            RemoveClientReactionAsync(reactionName, reactionId).GetAwaiter().GetResult();
        }

        public async Task RemoveReactionAsync(ulong userId, string reactionName, ulong? reactionId = null)
        {
            await Client.RemoveMessageReactionAsync(Channel.Id, Id, userId, reactionName, reactionId);
        }

        public void RemoveReaction(ulong userId, string reactionName, ulong? reactionId = null)
        {
            RemoveReactionAsync(userId, reactionName, reactionId).GetAwaiter().GetResult();
        }

        public async Task PinAsync()
        {
            await Channel.PinMessageAsync(Id);
        }

        public void Pin()
        {
            PinAsync().GetAwaiter().GetResult();
        }

        public async Task CrosspostAsync()
        {
            Update(await Client.CrosspostMessageAsync(Channel.Id, Id));
        }

        public void Crosspost()
        {
            CrosspostAsync().GetAwaiter().GetResult();
        }

        public static implicit operator ulong(DiscordMessage instance)
        {
            return instance.Id;
        }
    }
}
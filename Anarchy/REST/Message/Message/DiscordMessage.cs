using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

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


        [JsonProperty("id")]
        public ulong Id { get; private set; }


        [JsonProperty("content")]
        public string Content { get; private set; }


        [JsonProperty("tts")]
        public bool Tts { get; private set; }


        [JsonProperty("author")]
        private DiscordUser _authorUser;


        [JsonProperty("member")]
        private readonly GuildMember _authorMember;

        
        public MessageAuthor Author
        {
            get
            {
                if (_authorMember != null)
                    _authorMember.User = _authorUser;

                return new MessageAuthor(_authorUser, _authorMember);
            }
        }
        

        [JsonProperty("attachments")]
        private readonly IReadOnlyList<DiscordAttachment> _attachments;
        public DiscordAttachment Attachment
        {
            get { return _attachments == null || _attachments.Count == 0 ? null : _attachments[0]; }
        }


        [JsonProperty("embeds")]
        private IReadOnlyList<DiscordEmbed> _embeds;
        public DiscordEmbed Embed
        {
            get { return _embeds == null || _embeds.Count == 0 ? null : _embeds[0]; }
            private set { _embeds = new List<DiscordEmbed>() { value }; }
        }


        [JsonProperty("reactions")]
        public IReadOnlyList<MessageReaction> Reactions { get; private set; }


        [JsonProperty("mentions")]
        public IReadOnlyList<DiscordUser> Mentions { get; private set; }


        [JsonProperty("mention_roles")]
        public IReadOnlyList<ulong> MentionedRoles { get; private set; }


        [JsonProperty("mention_everyone")]
        public bool MentionedEveryone { get; private set; }


        [JsonProperty("timestamp")]
        public DateTime SentAt { get; private set; }


        [JsonProperty("edited_timestamp")]
        public DateTime? EditedAt { get; private set; }


        [JsonProperty("pinned")]
        public bool Pinned { get; private set; }


        [JsonProperty("channel_id")]
        private readonly ulong _channelId;

        public MinimalTextChannel Channel
        {
            get
            {
                return new MinimalTextChannel(_channelId).SetClient(Client);
            }
        }


        [JsonProperty("guild_id")]
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


        [JsonProperty("type")]
        public MessageType Type { get; private set; }


        [JsonProperty("flags")]
        public MessageFlags Flags { get; private set; }


        public async Task EditAsync(MessageEditProperties properties)
        {
            if (Type != MessageType.Default)
                throw new InvalidOperationException("Can only edit messages with a Type of 'Default'");

            DiscordMessage msg = await Client.EditMessageAsync(Channel.Id, Id, properties);
            Content = msg.Content;
            Pinned = msg.Pinned;
            Mentions = msg.Mentions;
            MentionedRoles = msg.MentionedRoles;
            MentionedEveryone = msg.MentionedEveryone;
            Embed = msg.Embed;
            msg.Flags = properties.Flags;
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


        public async Task<IReadOnlyList<DiscordUser>> GetReactionsAsync(string reaction, uint limit = 25, ulong afterId = 0)
        {
            return await Client.GetMessageReactionsAsync(Channel.Id, Id, reaction, limit, afterId);
        }

        /// <summary>
        /// Gets instances of a reaction to a message
        /// </summary>
        /// <param name="reaction">The reaction</param>
        /// <param name="limit">Max amount of reactions to receive</param>
        /// <param name="afterId">The reaction ID to offset from</param>
        public IReadOnlyList<DiscordUser> GetReactions(string reaction, uint limit = 25, ulong afterId = 0)
        {
            return GetReactionsAsync(reaction, limit, afterId).GetAwaiter().GetResult();
        }


        public async Task AddReactionAsync(string reaction)
        {
            await Client.AddMessageReactionAsync(Channel.Id, Id, reaction);
        }

        /// <summary>
        /// Adds a reaction to the message
        /// </summary>
        public void AddReaction(string reaction)
        {
            AddReactionAsync(reaction).GetAwaiter().GetResult();
        }


        public async Task RemoveClientReactionAsync(string reaction)
        {
            await Client.RemoveClientMessageReactionAsync(Channel.Id, Id, reaction);
        }

        public void RemoveClientReaction(string reaction)
        {
            RemoveClientReactionAsync(reaction).GetAwaiter().GetResult();
        }


        public async Task RemoveReactionAsync(string reaction, ulong userId)
        {
            await Client.RemoveMessageReactionAsync(Channel.Id, Id, reaction, userId);
        }

        public void RemoveReaction(string reaction, ulong userId)
        {
            RemoveReactionAsync(reaction, userId).GetAwaiter().GetResult();
        }


        public async Task PinAsync()
        {
            await Channel.PinMessageAsync(Id);
        }

        public void Pin()
        {
            PinAsync().GetAwaiter().GetResult();
        }


        public static implicit operator ulong(DiscordMessage instance)
        {
            return instance.Id;
        }
    }
}
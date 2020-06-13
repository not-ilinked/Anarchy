using Discord.Webhook;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public class MinimalGuild : ControllableEx // the only reason this has Ex is bcuz of LoginGuild, lol
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }

        public MinimalGuild()
        { }

        public MinimalGuild(ulong guildId)
        {
            Id = guildId;
        }


        /// <summary>
        /// Deletes the guild
        /// </summary>
        public void Delete()
        {
            Client.DeleteGuild(Id);
        }


        /// <summary>
        /// Leaves the guild
        /// </summary>
        public void Leave(bool lurking = false)
        {
            Client.LeaveGuild(Id, lurking);
        }


        /// <summary>
        /// Acknowledges all messages and pings in the guild
        /// </summary>
        public void AcknowledgeMessages()
        {
            Client.AcknowledgeGuildMessages(Id);
        }


        /// <summary>
        /// Changes the client's nickname for this guild
        /// </summary>
        /// <param name="nickname">New nickname</param>
        public void ChangeClientNickname(string nickname)
        {
            Client.ChangeClientNickname(Id, nickname);
        }

        public ClientGuildSettings ModifyClientSettings(ClientGuildProperties properties)
        {
            return Client.ModifyClientGuildSettings(Id, properties);
        }

        /// <summary>
        /// Gets the guild's templates
        /// </summary>
        public IReadOnlyList<DiscordGuildTemplate> GetTemplates()
        {
            return Client.GetGuildTemplates(Id);
        }


        /// <summary>
        /// Creates a template for the guild
        /// </summary>
        public DiscordGuildTemplate CreateTemplate(string name, string description)
        {
            return Client.CreateGuildTemplate(Id, name, description);
        }


        public DiscordGuildTemplate DeleteTemplate(string templateCode)
        {
            return Client.DeleteGuildTemplate(Id, templateCode);
        }


        public void SetVanityUrl(string vanityCode)
        {
            Client.SetGuildVanityUrl(Id, vanityCode);
        }


        /// <summary>
        /// Gets the guild's channels
        /// </summary>
        public virtual IReadOnlyList<GuildChannel> GetChannels()
        {
            return Client.GetGuildChannels(Id);
        }


        /// <summary>
        /// Creates a channel
        /// </summary>
        /// <param name="properties">Options for creating the channel</param>
        /// <returns>The created channel</returns>
        public GuildChannel CreateChannel(string name, ChannelType type, ulong? parentId = null)
        {
            return Client.CreateGuildChannel(Id, name, type, parentId);
        }


        /// <summary>
        /// Gets the guild's emojis
        /// </summary>
        public virtual IReadOnlyList<DiscordEmoji> GetEmojis()
        {
            return Client.GetGuildEmojis(Id);
        }


        /// <summary>
        /// Gets an emoji in the guild
        /// </summary>
        /// <param name="emojiId">ID of the emoji</param>
        public DiscordEmoji GetEmoji(ulong emojiId)
        {
            return Client.GetGuildEmoji(Id, emojiId);
        }


        /// <summary>
        /// Creates an emoji
        /// </summary>
        /// <param name="properties">Options for creating the emoji</param>
        public DiscordEmoji CreateEmoji(EmojiProperties properties)
        {
            return Client.CreateEmoji(Id, properties);
        }


        /// <summary>
        /// Gets the guild's roles
        /// </summary>
        public virtual IReadOnlyList<DiscordRole> GetRoles()
        {
            return Client.GetGuildRoles(Id);
        }


        /// <summary>
        /// Creates a role
        /// </summary>
        /// <param name="properties">Options for modifying the role after creating it</param>
        /// <returns>The created role</returns>
        public DiscordRole CreateRole(RoleProperties properties = null)
        {
            return Client.CreateRole(Id, properties);
        }


        /// <summary>
        /// Gets an invite
        /// </summary>
        public IReadOnlyList<GuildInvite> GetInvites()
        {
            return Client.GetGuildInvites(Id);
        }


        /// <summary>
        /// Gets the guild's webhooks
        /// </summary>
        public IReadOnlyList<DiscordWebhook> GetWebhooks()
        {
            return Client.GetGuildWebhooks(Id);
        }


        /// <summary>
        /// Gets the guild's audit log
        /// </summary>
        /// <param name="filters"></param>
        public IReadOnlyList<AuditLogEntry> GetAuditLog(AuditLogFilters filters = null)
        {
            return Client.GetAuditLog(Id, filters);
        }


        /// <summary>
        /// Gets a member from the server
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns></returns>
        public GuildMember GetMember(ulong userId)
        {
            return Client.GetGuildMember(Id, userId);
        }


        /// <summary>
        /// Gets the guild's banned users
        /// </summary>
        public IReadOnlyList<Ban> GetBans()
        {
            return Client.GetGuildBans(Id);
        }


        /// <summary>
        /// Gets a banned member
        /// </summary>
        /// <param name="userId">ID of the user</param>
        public Ban GetBan(ulong userId)
        {
            return Client.GetGuildBan(Id, userId);
        }


        public static implicit operator ulong(MinimalGuild instance)
        {
            return instance.Id;
        }
    }
}

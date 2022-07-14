using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Discord
{
    public class MinimalGuild : Controllable
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }

        public MinimalGuild()
        { }

        public MinimalGuild(ulong guildId)
        {
            Id = guildId;
        }


        public async Task ModifyAsync(GuildProperties properties)
        {
            await Client.ModifyGuildAsync(Id, properties);
        }

        /// <summary>
        /// Modifies the guild
        /// </summary>
        /// <param name="properties">Options for modifying the guild</param>
        public void Modify(GuildProperties properties)
        {
            ModifyAsync(properties).GetAwaiter().GetResult();
        }


        public async Task DeleteAsync()
        {
            await Client.DeleteGuildAsync(Id);
        }

        /// <summary>
        /// Deletes the guild
        /// </summary>
        public void Delete()
        {
            DeleteAsync().GetAwaiter().GetResult();
        }


        public async Task LeaveAsync(bool lurking = false)
        {
            await Client.LeaveGuildAsync(Id, lurking);
        }

        /// <summary>
        /// Leaves the guild
        /// </summary>
        public void Leave(bool lurking = false)
        {
            LeaveAsync(lurking).GetAwaiter().GetResult();
        }


        public async Task SetNicknameAsync(string nickname)
        {
            await Client.SetClientNicknameAsync(Id, nickname);
        }

        public void SetNickname(string nickname)
        {
            SetNicknameAsync(nickname).GetAwaiter().GetResult();
        }


        public async Task AcknowledgeMessagesAsync()
        {
            await Client.AcknowledgeGuildMessagesAsync(Id);
        }

        /// <summary>
        /// Acknowledges all messages and pings in the guild
        /// </summary>
        public void AcknowledgeMessages()
        {
            AcknowledgeMessagesAsync().GetAwaiter().GetResult();
        }


        public async Task<ClientGuildSettings> ModifyClientSettingsAsync(GuildSettingsProperties properties)
        {
            return await Client.ModifyGuildSettingsAsync(Id, properties);
        }

        public ClientGuildSettings ModifyClientSettings(GuildSettingsProperties properties)
        {
            return ModifyClientSettingsAsync(properties).GetAwaiter().GetResult();
        }


        public async Task<IReadOnlyList<DiscordGuildTemplate>> GetTemplatesAsync()
        {
            return await Client.GetGuildTemplatesAsync(Id);
        }

        /// <summary>
        /// Gets the guild's templates
        /// </summary>
        public IReadOnlyList<DiscordGuildTemplate> GetTemplates()
        {
            return GetTemplatesAsync().GetAwaiter().GetResult();
        }


        public async Task<DiscordGuildTemplate> CreateTemplateAsync(string name, string description)
        {
            return await Client.CreateGuildTemplateAsync(Id, name, description);
        }

        /// <summary>
        /// Creates a template for the guild
        /// </summary>
        public DiscordGuildTemplate CreateTemplate(string name, string description)
        {
            return CreateTemplateAsync(name, description).GetAwaiter().GetResult();
        }


        public async Task<DiscordGuildTemplate> DeleteTemplateAsync(string templateCode)
        {
            return await Client.DeleteGuildTemplateAsync(Id, templateCode);
        }

        public DiscordGuildTemplate DeleteTemplate(string templateCode)
        {
            return DeleteTemplateAsync(templateCode).GetAwaiter().GetResult();
        }


        public async Task SetVanityUrlAsync(string vanityCode)
        {
            await Client.SetGuildVanityUrlAsync(Id, vanityCode);
        }

        public void SetVanityUrl(string vanityCode)
        {
            SetVanityUrlAsync(vanityCode).GetAwaiter().GetResult();
        }


        public Task<WelcomeScreen> GetWelcomeScreenAsync()
        {
            return Client.GetWelcomeScreenAsync(Id);
        }

        public WelcomeScreen GetWelcomeScreen()
        {
            return GetWelcomeScreenAsync().GetAwaiter().GetResult();
        }


        public Task<WelcomeScreen> ModifyWelcomeScreenAsync(WelcomeScreenProperties properties)
        {
            return Client.ModifyWelcomeScreenAsync(Id, properties);
        }

        public WelcomeScreen ModifyWelcomeScreen(WelcomeScreenProperties properties)
        {
            return ModifyWelcomeScreenAsync(properties).GetAwaiter().GetResult();
        }


        public Task<GuildVerificationForm> GetVerificationFormAsync(string inviteCode) => Client.GetGuildVerificationFormAsync(Id, inviteCode);
        public GuildVerificationForm GetVerificationForm(string inviteCode) => GetVerificationFormAsync(inviteCode).GetAwaiter().GetResult();

        public Task<GuildVerificationForm> ModifyVerificationFormAsync(VerificationFormProperties properties) => Client.ModifyGuildVerificationFormAsync(Id, properties);
        public GuildVerificationForm ModifyVerificationForm(VerificationFormProperties properties) => ModifyVerificationFormAsync(properties).GetAwaiter().GetResult();


        public virtual async Task<IReadOnlyList<GuildChannel>> GetChannelsAsync()
        {
            return await Client.GetGuildChannelsAsync(Id);
        }

        /// <summary>
        /// Gets the guild's channels
        /// </summary>
        public virtual IReadOnlyList<GuildChannel> GetChannels()
        {
            return GetChannelsAsync().GetAwaiter().GetResult();
        }


        public async Task<GuildChannel> CreateChannelAsync(string name, ChannelType type, ulong? parentId = null)
        {
            return await Client.CreateGuildChannelAsync(Id, name, type, parentId);
        }

        /// <summary>
        /// Creates a channel
        /// </summary>
        /// <param name="properties">Options for creating the channel</param>
        /// <returns>The created channel</returns>
        public GuildChannel CreateChannel(string name, ChannelType type, ulong? parentId = null)
        {
            return CreateChannelAsync(name, type, parentId).GetAwaiter().GetResult();
        }


        public virtual async Task<IReadOnlyList<DiscordEmoji>> GetEmojisAsync()
        {
            return await Client.GetGuildEmojisAsync(Id);
        }

        /// <summary>
        /// Gets the guild's emojis
        /// </summary>
        public virtual IReadOnlyList<DiscordEmoji> GetEmojis()
        {
            return GetEmojisAsync().GetAwaiter().GetResult();
        }


        public async Task<DiscordEmoji> GetEmojiAsync(ulong emojiId)
        {
            return await Client.GetGuildEmojiAsync(Id, emojiId);
        }

        /// <summary>
        /// Gets an emoji in the guild
        /// </summary>
        /// <param name="emojiId">ID of the emoji</param>
        public DiscordEmoji GetEmoji(ulong emojiId)
        {
            return GetEmojiAsync(emojiId).GetAwaiter().GetResult();
        }


        public async Task<DiscordEmoji> CreateEmojiAsync(EmojiProperties properties)
        {
            return await Client.CreateEmojiAsync(Id, properties);
        }

        /// <summary>
        /// Creates an emoji
        /// </summary>
        /// <param name="properties">Options for creating the emoji</param>
        public DiscordEmoji CreateEmoji(EmojiProperties properties)
        {
            return CreateEmojiAsync(properties).GetAwaiter().GetResult();
        }


        public virtual async Task<IReadOnlyList<DiscordRole>> GetRolesAsync()
        {
            return await Client.GetGuildRolesAsync(Id);
        }

        /// <summary>
        /// Gets the guild's roles
        /// </summary>
        public virtual IReadOnlyList<DiscordRole> GetRoles()
        {
            return GetRolesAsync().GetAwaiter().GetResult();
        }


        public async Task<DiscordRole> CreateRoleAsync(RoleProperties properties = null)
        {
            return await Client.CreateRoleAsync(Id, properties);
        }

        /// <summary>
        /// Creates a role
        /// </summary>
        /// <param name="properties">Options for modifying the role after creating it</param>
        /// <returns>The created role</returns>
        public DiscordRole CreateRole(RoleProperties properties = null)
        {
            return CreateRoleAsync(properties).GetAwaiter().GetResult();
        }


        public virtual async Task<IReadOnlyList<DiscordRole>> SetRolePositionsAsync(List<RolePositionUpdate> roles)
        {
            return await Client.SetRolePositionsAsync(Id, roles);
        }

        public virtual IReadOnlyList<DiscordRole> SetRolePositions(List<RolePositionUpdate> roles)
        {
            return SetRolePositionsAsync(roles).GetAwaiter().GetResult();
        }


        public async Task<IReadOnlyList<GuildInvite>> GetInvitesAsync()
        {
            return await Client.GetGuildInvitesAsync(Id);
        }

        public IReadOnlyList<GuildInvite> GetInvites()
        {
            return GetInvitesAsync().GetAwaiter().GetResult();
        }


        public async Task<IReadOnlyList<DiscordWebhook>> GetWebhooksAsync()
        {
            return await Client.GetGuildWebhooksAsync(Id);
        }

        public IReadOnlyList<DiscordWebhook> GetWebhooks()
        {
            return GetWebhooksAsync().GetAwaiter().GetResult();
        }


        public async Task<IReadOnlyList<AuditLogEntry>> GetAuditLogAsync(AuditLogFilters filters = null)
        {
            return await Client.GetAuditLogAsync(Id, filters);
        }

        /// <summary>
        /// Gets the guild's audit log
        /// </summary>
        /// <param name="filters"></param>
        public IReadOnlyList<AuditLogEntry> GetAuditLog(AuditLogFilters filters = null)
        {
            return GetAuditLogAsync(filters).GetAwaiter().GetResult();
        }


        public async Task<GuildMember> GetMemberAsync(ulong userId)
        {
            return await Client.GetGuildMemberAsync(Id, userId);
        }

        /// <summary>
        /// Gets a member from the server
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns></returns>
        public GuildMember GetMember(ulong userId)
        {
            return GetMemberAsync(userId).GetAwaiter().GetResult();
        }


        public async Task<IReadOnlyList<DiscordBan>> GetBansAsync()
        {
            return await Client.GetGuildBansAsync(Id);
        }

        /// <summary>
        /// Gets the guild's banned users
        /// </summary>
        public IReadOnlyList<DiscordBan> GetBans()
        {
            return GetBansAsync().GetAwaiter().GetResult();
        }


        public async Task<DiscordBan> GetBanAsync(ulong userId)
        {
            return await Client.GetGuildBanAsync(Id, userId);
        }

        /// <summary>
        /// Gets a banned member
        /// </summary>
        /// <param name="userId">ID of the user</param>
        public DiscordBan GetBan(ulong userId)
        {
            return GetBanAsync(userId).GetAwaiter().GetResult();
        }


        public async Task BanMemberAsync(ulong userId, string reason = null, uint deleteMessageDays = 0)
        {
            await Client.BanGuildMemberAsync(Id, userId, reason, deleteMessageDays);
        }

        public void BanMember(ulong userId, string reason = null, uint deleteMessageDays = 0)
        {
            BanMemberAsync(userId, reason, deleteMessageDays).GetAwaiter().GetResult();
        }


        public static implicit operator ulong(MinimalGuild instance)
        {
            return instance.Id;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Gateway;
using Newtonsoft.Json;

namespace Discord
{
    public class GuildMember : PartialGuildMember, IDisposable, IMentionable
    {
        [JsonProperty("nick")]
        public string Nickname { get; private set; }

        [JsonProperty("roles")]
        public IReadOnlyList<ulong> Roles { get; private set; }

        [JsonProperty("joined_at")]
        public DateTime JoinedAt { get; internal set; }

        [JsonProperty("premium_since")]
        public DateTime? BoostingSince { get; private set; }

        /// <summary>
        /// Updates the member's information
        /// </summary>
        protected void Update(GuildMember member)
        {
            User = member.User;
            Nickname = member.Nickname;
            Roles = member.Roles;
            BoostingSince = member.BoostingSince;
        }

        public async Task UpdateAsync()
        {
            Update(await Client.GetGuildMemberAsync(GuildId, User.Id));
        }

        public async Task ModifyAsync(GuildMemberProperties properties)
        {
            await Client.ModifyGuildMemberAsync(GuildId, User.Id, properties);

            if (properties.NickProperty.Set)
                Nickname = properties.Nickname;

            if (properties.RoleProperty.Set)
                Roles = properties.Roles;
        }

        /// <summary>
        /// Modifies the specified guild member
        /// </summary>
        /// <param name="properties">Things to change</param>
        public void Modify(GuildMemberProperties properties)
        {
            ModifyAsync(properties).GetAwaiter().GetResult();
        }

        public async Task AddRoleAsync(ulong roleId)
        {
            await Client.AddRoleToUserAsync(GuildId, roleId, User.Id);
        }

        /// <summary>
        /// Adds a role to the guild member
        /// </summary>
        /// <param name="roleId">ID of the role</param>
        public void AddRole(ulong roleId)
        {
            AddRoleAsync(roleId).GetAwaiter().GetResult();
        }

        public async Task RemoveRoleAsync(ulong roleId)
        {
            await Client.RemoveRoleFromUserAsync(GuildId, roleId, User.Id);
        }

        /// <summary>
        /// Removes a role from the guild member
        /// </summary>
        /// <param name="roleId">ID of the role</param>
        public void RemoveRole(ulong roleId)
        {
            RemoveRoleAsync(roleId).GetAwaiter().GetResult();
        }

        private async Task<DiscordGuild> SeekGuildAsync()
        {
            if (Client.GetType() == typeof(DiscordSocketClient))
            {
                var socketClient = (DiscordSocketClient) Client;

                if (socketClient.Config.Cache)
                    return socketClient.GetCachedGuild(GuildId);
            }

            return await Client.GetGuildAsync(GuildId);
        }

        private DiscordPermission ComputePermissions(DiscordGuild guild)
        {
            DiscordPermission permissions = DiscordPermission.None;

            if (guild.OwnerId == User.Id)
                permissions = PermissionUtils.GetAllPermissions();
            else
            {
                foreach (var role in guild.Roles.Where(r => Roles.Contains(r.Id) || r.Name == "@everyone"))
                    permissions = permissions.Add(role.Permissions);

                if (permissions.Has(DiscordPermission.Administrator))
                    permissions = PermissionUtils.GetAllPermissions();
            }

            return permissions;
        }

        public async Task<DiscordPermission> GetPermissionsAsync()
        {
            return ComputePermissions(await SeekGuildAsync());
        }

        /// <summary>
        /// Gets the user's permissions in the guild
        /// </summary>
        /// <returns></returns>
        public DiscordPermission GetPermissions()
        {
            return GetPermissionsAsync().GetAwaiter().GetResult();
        }

        public async Task<DiscordPermission> GetPermissionsAsync(IEnumerable<DiscordPermissionOverwrite> affectedBy)
        {
            var guild = await SeekGuildAsync();

            var perms = ComputePermissions(guild);

            if (guild.OwnerId != User.Id && !perms.Has(DiscordPermission.Administrator))
            {
                foreach (var overwrite in affectedBy)
                {
                    if (overwrite.Type == PermissionOverwriteType.Role && overwrite.AffectedId == guild.EveryoneRole.Id || Roles.Contains(overwrite.AffectedId) || (overwrite.Type == PermissionOverwriteType.Member && overwrite.AffectedId == User.Id))
                        perms = perms.Remove(overwrite.Deny).Add(overwrite.Allow);
                }
            }

            return perms;
        }

        public DiscordPermission GetPermissions(IEnumerable<DiscordPermissionOverwrite> affectedBy)
        {
            return GetPermissionsAsync(affectedBy).GetAwaiter().GetResult();
        }

        public async Task KickAsync()
        {
            await Client.KickGuildMemberAsync(GuildId, User.Id);
        }

        /// <summary>
        /// Kicks the member from the guild
        /// </summary>
        public void Kick()
        {
            KickAsync().GetAwaiter().GetResult();
        }

        public async Task BanAsync(string reason = null, uint deleteMessageDays = 0)
        {
            await Client.BanGuildMemberAsync(GuildId, User.Id, reason, deleteMessageDays);
        }

        /// <summary>
        /// Bans the member from the guild
        /// </summary>
        /// <param name="reason">The reason for banning the user</param>
        /// <param name="deleteMessageDays">Amount of days to purge messages (max is 7)</param>
        public void Ban(string reason = null, uint deleteMessageDays = 0)
        {
            BanAsync(reason, deleteMessageDays).GetAwaiter().GetResult();
        }

        public async Task UnbanAsync()
        {
            await Client.UnbanGuildMemberAsync(GuildId, User.Id);
        }

        /// <summary>
        /// Unbans the user from the guild
        /// </summary>
        public void Unban()
        {
            UnbanAsync().GetAwaiter().GetResult();
        }

        public string AsMessagable()
        {
            return User.AsMessagable();
        }

        public override string ToString()
        {
            return User.ToString();
        }

        public static implicit operator ulong(GuildMember instance)
        {
            return instance.User.Id;
        }

        public new void Dispose()
        {
            Nickname = null;
            Roles = null;
            BoostingSince = null;
            base.Dispose();
        }
    }
}
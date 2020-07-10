using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discord
{
    public static class RoleExtensions
    {
        #region management
        public static async Task<DiscordRole> CreateRoleAsync(this DiscordClient client, ulong guildId, RoleProperties properties = null)
        {
            DiscordRole role = (await client.HttpClient.PostAsync($"/guilds/{guildId}/roles"))
                                    .Deserialize<DiscordRole>().SetClient(client);
            role.GuildId = guildId;
            if (properties != null)
                role.Modify(properties);
            return role;
        }

        /// <summary>
        /// Creates a role
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="properties">Options for modifying the created ole</param>
        /// <returns>The created <see cref="DiscordRole"/></returns>
        public static DiscordRole CreateRole(this DiscordClient client, ulong guildId, RoleProperties properties = null)
        {
            return client.CreateRoleAsync(guildId, properties).Result;
        }


        public static async Task<DiscordRole> ModifyRoleAsync(this DiscordClient client, ulong guildId, ulong roleId, RoleProperties properties)
        {
            DiscordRole changed = (await client.HttpClient.PatchAsync($"/guilds/{guildId}/roles/{roleId}", properties)).Deserialize<DiscordRole>().SetClient(client);
            changed.GuildId = guildId;
            return changed;
        }

        /// <summary>
        /// Modifies a role
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="roleId">ID of the role</param>
        /// <param name="properties"></param>
        /// <returns>The modified <see cref="DiscordRole"/></returns>
        public static DiscordRole ModifyRole(this DiscordClient client, ulong guildId, ulong roleId, RoleProperties properties)
        {
            return client.ModifyRoleAsync(guildId, roleId, properties).Result;
        }


        public static async Task DeleteRoleAsync(this DiscordClient client, ulong guildId, ulong roleId)
        {
            await client.HttpClient.DeleteAsync($"/guilds/{guildId}/roles/{roleId}");
        }

        /// <summary>
        /// Deletes a role
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="roleId">ID of the role</param>
        public static void DeleteRole(this DiscordClient client, ulong guildId, ulong roleId)
        {
            client.DeleteRoleAsync(guildId, roleId).GetAwaiter().GetResult();
        }
        #endregion


        public static async Task SetGuildMemberRolesAsync(this DiscordClient client, ulong guildId, ulong userId, List<ulong> roles)
        {
            await client.ModifyGuildMemberAsync(guildId, userId, new GuildMemberProperties() { Roles = roles });
        }

        /// <summary>
        /// Sets a guild member's roles
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="userId">ID of the user</param>
        public static void SetGuildMemberRoles(this DiscordClient client, ulong guildId, ulong userId, List<ulong> roles)
        {
            client.SetGuildMemberRolesAsync(guildId, userId, roles).GetAwaiter().GetResult();
        }


        public static async Task AddRoleToUserAsync(this DiscordClient client, ulong guildId, ulong roleId, ulong userId)
        {
            await client.HttpClient.PutAsync($"/guilds/{guildId}/members/{userId}/roles/{roleId}");
        }

        /// <summary>
        /// Adds a role to a guild member
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="roleId">ID of the role</param>
        /// <param name="userId">ID of the guild member</param>
        public static void AddRoleToUser(this DiscordClient client, ulong guildId, ulong roleId, ulong userId)
        {
            client.AddRoleToUserAsync(guildId, roleId, userId).GetAwaiter().GetResult();
        }


        public static async Task RemoveRoleFromUserAsync(this DiscordClient client, ulong guildId, ulong roleId, ulong userId)
        {
            await client.HttpClient.DeleteAsync($"/guilds/{guildId}/members/{userId}/roles/{roleId}");
        }

        /// <summary>
        /// Removes a role from a guild member
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="roleId">ID of the role</param>
        /// <param name="userId">ID of the guild member</param>
        public static void RemoveRoleFromUser(this DiscordClient client, ulong guildId, ulong roleId, ulong userId)
        {
            client.RemoveRoleFromUserAsync(guildId, roleId, userId).GetAwaiter().GetResult();
        }


        public static async Task<IReadOnlyList<DiscordRole>> GetGuildRolesAsync(this DiscordClient client, ulong guildId)
        {
            IReadOnlyList<DiscordRole> roles = (await client.HttpClient.GetAsync($"/guilds/{guildId}/roles"))
                                                    .Deserialize<IReadOnlyList<DiscordRole>>().SetClientsInList(client);
            foreach (var role in roles)
                role.GuildId = guildId;
            return roles;
        }

        /// <summary>
        /// Gets a guild's roles
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static IReadOnlyList<DiscordRole> GetGuildRoles(this DiscordClient client, ulong guildId)
        {
            return client.GetGuildRolesAsync(guildId).Result;
        }
    }
}
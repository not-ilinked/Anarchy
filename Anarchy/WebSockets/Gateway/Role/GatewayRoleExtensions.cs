using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord.Gateway
{
    public static class GatewayRoleExtensions
    {
        public static async Task<IReadOnlyList<DiscordRole>> GetGuildRolesAsync(this DiscordSocketClient client, ulong guildId)
        {
            if (client.Config.Cache)
                return client.GetCachedGuild(guildId).Roles;
            else
                return await ((DiscordClient) client).GetGuildRolesAsync(guildId);
        }

        /// <summary>
        /// Gets a guild's roles
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static IReadOnlyList<DiscordRole> GetGuildRoles(this DiscordSocketClient client, ulong guildId)
        {
            return client.GetGuildRolesAsync(guildId).GetAwaiter().GetResult();
        }

        public static DiscordRole GetGuildRole(this DiscordSocketClient client, ulong roleId)
        {
            if (!client.Config.Cache)
                throw new NotSupportedException("Caching is disabled for this client.");

            foreach (var guild in client.GetCachedGuilds())
            {
                foreach (var role in guild.Roles)
                {
                    if (role.Id == roleId)
                        return role;
                }
            }

            throw new DiscordHttpException(new DiscordHttpError(DiscordError.UnknownRole, "Role was not found in cache"));
        }
    }
}

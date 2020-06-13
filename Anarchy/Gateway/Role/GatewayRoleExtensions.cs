using System;
using System.Collections.Generic;
using System.Linq;

namespace Discord.Gateway
{
    public static class GatewayRoleExtensions
    {
        /// <summary>
        /// Gets a guild's roles
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static IReadOnlyList<DiscordRole> GetGuildRoles(this DiscordSocketClient client, ulong guildId)
        {
            if (client.Config.Cache)
                return client.GetCachedGuild(guildId).Roles;
            else
                return ((DiscordClient)client).GetGuildRoles(guildId);
        }


        /// <summary>
        /// Gets a specific role from a guild.
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="roleId">ID of the role</param>
        public static DiscordRole GetGuildRole(this DiscordSocketClient client, ulong guildId, ulong roleId)
        {
            if (client.Config.Cache)
            {
                try
                {
                    return client.GetGuildRoles(guildId).First(r => r.Id == roleId);
                }
                catch (InvalidOperationException)
                {
                    throw new DiscordHttpException(client, new DiscordHttpError(DiscordError.UnknownRole, "Role was not found in cache"));
                }
            }
            else
                return ((DiscordClient)client).GetGuildRole(guildId, roleId);
        }
    }
}

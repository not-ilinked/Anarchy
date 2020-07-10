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
    }
}

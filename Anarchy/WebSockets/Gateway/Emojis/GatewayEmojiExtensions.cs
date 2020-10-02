using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discord.Gateway
{
    public static class GatewayEmojiExtensions
    {
        public static async Task<IReadOnlyList<DiscordEmoji>> GetGuildEmojisAsync(this DiscordSocketClient client, ulong guildId)
        {
            if (client.Config.Cache)
                return client.GetCachedGuild(guildId).Emojis;
            else
                return await ((DiscordClient)client).GetGuildEmojisAsync(guildId);
        }

        /// <summary>
        /// Gets the guild's emojis
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static IReadOnlyList<DiscordEmoji> GetGuildEmojis(this DiscordSocketClient client, ulong guildId)
        {
            return client.GetGuildEmojisAsync(guildId).GetAwaiter().GetResult();
        }
    }
}

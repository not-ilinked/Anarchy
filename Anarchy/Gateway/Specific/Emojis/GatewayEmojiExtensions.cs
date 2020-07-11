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


        public static async Task<DiscordEmoji> GetGuildEmojiAsync(this DiscordSocketClient client, ulong guildId, ulong emojiId)
        {
            if (client.Config.Cache)
            {
                try
                {
                    return (await client.GetGuildEmojisAsync(guildId)).First(e => e.Id == emojiId);
                }
                catch
                {
                    throw new DiscordHttpException(client, new DiscordHttpError(DiscordError.UnknownRole, "Emoji was not found in cache"));
                }
            }
            else
                return await ((DiscordClient)client).GetGuildEmojiAsync(guildId, emojiId);
        }

        /// <summary>
        /// Gets an emoji
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="emojiId">ID of the emoji</param>
        public static DiscordEmoji GetGuildEmoji(this DiscordSocketClient client, ulong guildId, ulong emojiId)
        {
            return client.GetGuildEmojiAsync(guildId, emojiId).GetAwaiter().GetResult();
        }
    }
}

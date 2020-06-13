using System;
using System.Collections.Generic;
using System.Linq;

namespace Discord.Gateway
{
    public static class GatewayEmojiExtensions
    {
        /// <summary>
        /// Gets the guild's emojis
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static IReadOnlyList<DiscordEmoji> GetGuildEmojis(this DiscordSocketClient client, ulong guildId)
        {
            if (client.Config.Cache)
                return client.GetCachedGuild(guildId).Emojis;
            else
                return ((DiscordClient)client).GetGuildEmojis(guildId);
        }


        /// <summary>
        /// Gets an emoji
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="emojiId">ID of the emoji</param>
        public static DiscordEmoji GetGuildEmoji(this DiscordSocketClient client, ulong guildId, ulong emojiId)
        {
            if (client.Config.Cache)
            {
                try
                {
                    return client.GetGuildEmojis(guildId).First(e => e.Id == emojiId);
                }
                catch
                {
                    throw new DiscordHttpException(client, new DiscordHttpError(DiscordError.UnknownRole, "Emoji was not found in cache"));
                }
            }
            else
                return ((DiscordClient)client).GetGuildEmoji(guildId, emojiId);
        }
    }
}

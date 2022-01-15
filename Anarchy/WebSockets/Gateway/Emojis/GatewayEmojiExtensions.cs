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
            {
                return client.GetCachedGuild(guildId).Emojis;
            }
            else
            {
                return await ((DiscordClient)client).GetGuildEmojisAsync(guildId);
            }
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
                    return client.GetCachedGuild(guildId).Emojis.First(e => e.Id == emojiId);
                }
                catch (InvalidOperationException)
                {
                    throw new DiscordHttpException(new DiscordHttpError(DiscordError.UnknownEmoji, "Emoji was not found in cache"));
                }
            }
            else
            {
                return await ((DiscordClient)client).GetGuildEmojiAsync(guildId, emojiId);
            }
        }

        public static DiscordEmoji GetGuildEmoji(this DiscordSocketClient client, ulong guildId, ulong emojiId)
        {
            return client.GetGuildEmojiAsync(guildId, emojiId).GetAwaiter().GetResult();
        }


        public static DiscordEmoji GetGuildEmoji(this DiscordSocketClient client, ulong emojiId)
        {
            if (!client.Config.Cache)
            {
                throw new NotSupportedException("Caching is disabled for this client.");
            }

            foreach (SocketGuild guild in client.GetCachedGuilds())
            {
                foreach (DiscordEmoji emoji in guild.Emojis)
                {
                    if (emoji.Id == emojiId)
                    {
                        return emoji;
                    }
                }
            }

            throw new DiscordHttpException(new DiscordHttpError(DiscordError.UnknownEmoji, "Emoji was not found in cache"));
        }
    }
}

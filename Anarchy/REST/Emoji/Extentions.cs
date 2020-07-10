using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Discord
{
    public static class EmojiExtentions
    {
        #region management
        public static async Task<DiscordEmoji> CreateEmojiAsync(this DiscordClient client, ulong guildId, EmojiProperties properties)
        {
            DiscordEmoji emoji = (await client.HttpClient.PostAsync($"/guilds/{guildId}/emojis", properties)).Deserialize<DiscordEmoji>().SetClient(client);
            emoji.GuildId = guildId;
            return emoji;
        }

        /// <summary>
        /// Creates an emoji
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="properties">Options for creating the emoji</param>
        /// <returns>The created <see cref="DiscordEmoji"/></returns>
        public static DiscordEmoji CreateEmoji(this DiscordClient client, ulong guildId, EmojiProperties properties)
        {
            return client.CreateEmojiAsync(guildId, properties).GetAwaiter().GetResult();
        }


        public static async Task<DiscordEmoji> ModifyEmojiAsync(this DiscordClient client, ulong guildId, ulong emojiId, string name)
        {
            return (await client.HttpClient.PatchAsync($"/guilds/{guildId}/emojis/{emojiId}", $"{{\"name\":\"{name}\"}}"))
                                .Deserialize<DiscordEmoji>().SetClient(client);
        }

        /// <summary>
        /// Modifies an emoji
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="emojiId">ID of the emoji</param>
        /// <param name="name">The emoji's new name</param>
        /// <returns>The moeified <see cref="DiscordEmoji"/></returns>
        public static DiscordEmoji ModifyEmoji(this DiscordClient client, ulong guildId, ulong emojiId, string name)
        {
            return client.ModifyEmojiAsync(guildId, emojiId, name).GetAwaiter().GetResult();
        }


        public static async Task DeleteEmojiAsync(this DiscordClient client, ulong guildId, ulong emojiId)
        {
            await client.HttpClient.DeleteAsync($"/guilds/{guildId}/emojis/{emojiId}");
        }

        /// <summary>
        /// Deletes an emoji
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="emojiId">ID of the emoji</param>
        public static void DeleteEmoji(this DiscordClient client, ulong guildId, ulong emojiId)
        {
            client.DeleteEmojiAsync(guildId, emojiId).GetAwaiter().GetResult();
        }
        #endregion


        public static async Task<IReadOnlyList<DiscordEmoji>> GetGuildEmojisAsync(this DiscordClient client, ulong guildId)
        {
            var emojis = (await client.HttpClient.GetAsync($"/guilds/{guildId}/emojis"))
                                        .Deserialize<IReadOnlyList<DiscordEmoji>>().SetClientsInList(client);
            foreach (var emoji in emojis) emoji.GuildId = guildId;
            return emojis;
        }

        /// <summary>
        /// Gets the guild's emojis
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static IReadOnlyList<DiscordEmoji> GetGuildEmojis(this DiscordClient client, ulong guildId)
        {
            return client.GetGuildEmojisAsync(guildId).GetAwaiter().GetResult();
        }


        public static async Task<DiscordEmoji> GetGuildEmojiAsync(this DiscordClient client, ulong guildId, ulong emojiId)
        {
            DiscordEmoji reaction = (await client.HttpClient.GetAsync($"/guilds/{guildId}/emojis/{emojiId}"))
                                        .Deserialize<DiscordEmoji>().SetClient(client);
            reaction.GuildId = guildId;
            return reaction;
        }

        /// <summary>
        /// Gets an emoji
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="emojiId">ID of the emoji</param>
        public static DiscordEmoji GetGuildEmoji(this DiscordClient client, ulong guildId, ulong emojiId)
        {
            return client.GetGuildEmojiAsync(guildId, emojiId).GetAwaiter().GetResult();
        }


        public static async Task AddMessageReactionAsync(this DiscordClient client, ulong channelId, ulong messageId, string reaction)
        {
            await client.HttpClient.PutAsync($"/channels/{channelId}/messages/{messageId}/reactions/{reaction}/@me");
        }


        /// <summary>
        /// Adds a reaction to a message
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message</param>
        /// <param name="reaction">The reaction to add</param>
        public static void AddMessageReaction(this DiscordClient client, ulong channelId, ulong messageId, string reaction)
        {
            client.AddMessageReactionAsync(channelId, messageId, reaction).GetAwaiter().GetResult();
        }


        public static async Task RemoveMessageReactionAsync(this DiscordClient client, ulong channelId, ulong messageId, string reaction, ulong userId = 0)
        {
            string user = "@me";

            if (userId != 0)
                user = userId.ToString();

            await client.HttpClient.DeleteAsync($"/channels/{channelId}/messages/{messageId}/reactions/{reaction}/{user}");
        }

        /// <summary>
        /// Removes a reaction from a message.
        /// If userId is not set, the client's own reaction is removed
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message</param>
        /// <param name="userId">User who's reaction should be removed</param>
        /// <param name="reaction">The reaction to remove</param>
        public static void RemoveMessageReaction(this DiscordClient client, ulong channelId, ulong messageId, string reaction, ulong userId = 0)
        {
            client.RemoveMessageReaction(channelId, messageId, reaction, userId);
        }
    }
}
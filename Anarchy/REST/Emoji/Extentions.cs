using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public static class EmojiExtentions
    {
        #region management
        /// <summary>
        /// Creates an emoji
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="properties">Options for creating the emoji</param>
        /// <returns>The created <see cref="DiscordEmoji"/></returns>
        public static DiscordEmoji CreateEmoji(this DiscordClient client, ulong guildId, EmojiProperties properties)
        {
            DiscordEmoji emoji = client.HttpClient.Post($"/guilds/{guildId}/emojis", properties).Deserialize<DiscordEmoji>().SetClient(client);
            emoji.GuildId = guildId;
            return emoji;
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
            return client.HttpClient.Patch($"/guilds/{guildId}/emojis/{emojiId}", $"{{\"name\":\"{name}\"}}")
                                .Deserialize<DiscordEmoji>().SetClient(client);
        }


        /// <summary>
        /// Deletes an emoji
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="emojiId">ID of the emoji</param>
        public static void DeleteEmoji(this DiscordClient client, ulong guildId, ulong emojiId)
        {
            client.HttpClient.Delete($"/guilds/{guildId}/emojis/{emojiId}");
        }
        #endregion


        /// <summary>
        /// Gets the guild's emojis
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static IReadOnlyList<DiscordEmoji> GetGuildEmojis(this DiscordClient client, ulong guildId)
        {
            var emojis = client.HttpClient.Get($"/guilds/{guildId}/emojis")
                                        .Deserialize<IReadOnlyList<DiscordEmoji>>().SetClientsInList(client);
            foreach (var emoji in emojis) emoji.GuildId = guildId;
            return emojis;
        }


        /// <summary>
        /// Gets an emoji
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="emojiId">ID of the emoji</param>
        public static DiscordEmoji GetGuildEmoji(this DiscordClient client, ulong guildId, ulong emojiId)
        {
            DiscordEmoji reaction = client.HttpClient.Get($"/guilds/{guildId}/emojis/{emojiId}")
                                        .Deserialize<DiscordEmoji>().SetClient(client);
            reaction.GuildId = guildId;
            return reaction;
        }


        /// <summary>
        /// Adds a reaction to a message
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="messageId">ID of the message</param>
        /// <param name="reaction">The reaction to add</param>
        public static void AddMessageReaction(this DiscordClient client, ulong channelId, ulong messageId, string reaction)
        {
            client.HttpClient.Put($"/channels/{channelId}/messages/{messageId}/reactions/{reaction}/@me");
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
            string user = "@me";

            if (userId != 0)
                user = userId.ToString();

            client.HttpClient.Delete($"/channels/{channelId}/messages/{messageId}/reactions/{reaction}/{user}");
        }
    }
}
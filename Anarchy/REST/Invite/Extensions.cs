using System.Collections.Generic;

namespace Discord
{
    public static class InviteExtensions
    {
        /// <summary>
        /// Creates an invite for a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="properties">Options for creating the invite</param>
        /// <returns>The created invite</returns>
        public static DiscordInvite CreateInvite(this DiscordClient client, ulong channelId, InviteProperties properties = null)
        {
            if (properties == null)
                properties = new InviteProperties();

            return client.HttpClient.Post($"/channels/{channelId}/invites", properties)
                                .Deserialize<DiscordInvite>().SetClient(client);
        }


        /// <summary>
        /// Deletes an invite
        /// </summary>
        /// <param name="invCode">The invite's code</param>
        /// <returns>The deleted invite</returns>
        public static DiscordInvite DeleteInvite(this DiscordClient client, string invCode)
        {
            return client.HttpClient.Delete($"/invites/{invCode}")
                                .Deserialize<DiscordInvite>().SetClient(client);
        }


        /// <summary>
        /// Gets an invite
        /// </summary>
        public static DiscordInvite GetInvite(this DiscordClient client, string invCode)
        {
            return client.HttpClient.Get($"/invites/{invCode}?with_counts=true")
                                .DeserializeEx<DiscordInvite>().SetClient(client);
        }


        /// <summary>
        /// Gets a guild's invites
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static IReadOnlyList<GuildInvite> GetGuildInvites(this DiscordClient client, ulong guildId)
        {
            return client.HttpClient.Get($"/guilds/{guildId}/invites")
                                .DeserializeExArray<GuildInvite>().SetClientsInList(client);
        }
    }
}
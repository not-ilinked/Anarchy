using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord
{
    public static class InviteExtensions
    {
        public static async Task<DiscordInvite> CreateInviteAsync(this DiscordClient client, ulong channelId, InviteProperties properties = null)
        {
            if (properties == null)
                properties = new InviteProperties();

            return (await client.HttpClient.PostAsync($"/channels/{channelId}/invites", properties))
                                    .ParseDeterministic<DiscordInvite>().SetClient(client);
        }

        /// <summary>
        /// Creates an invite for a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="properties">Options for creating the invite</param>
        /// <returns>The created invite</returns>
        public static DiscordInvite CreateInvite(this DiscordClient client, ulong channelId, InviteProperties properties = null)
        {
            return client.CreateInviteAsync(channelId, properties).GetAwaiter().GetResult();
        }


        public static async Task<DiscordInvite> DeleteInviteAsync(this DiscordClient client, string invCode)
        {
            return (await client.HttpClient.DeleteAsync($"/invites/{invCode}"))
                                    .ParseDeterministic<DiscordInvite>().SetClient(client);
        }

        /// <summary>
        /// Deletes an invite
        /// </summary>
        /// <param name="invCode">The invite's code</param>
        /// <returns>The deleted invite</returns>
        public static DiscordInvite DeleteInvite(this DiscordClient client, string invCode)
        {
            return client.DeleteInviteAsync(invCode).GetAwaiter().GetResult();
        }


        public static async Task<DiscordInvite> GetInviteAsync(this DiscordClient client, string invCode)
        {
            return (await client.HttpClient.GetAsync($"/invites/{invCode}?with_counts=true"))
                                      .ParseDeterministic<DiscordInvite>().SetClient(client);
        }

        /// <summary>
        /// Gets an invite
        /// </summary>
        public static DiscordInvite GetInvite(this DiscordClient client, string invCode)
        {
            return client.GetInviteAsync(invCode).GetAwaiter().GetResult();
        }


        public static async Task<IReadOnlyList<GuildInvite>> GetGuildInvitesAsync(this DiscordClient client, ulong guildId)
        {
            return (await client.HttpClient.GetAsync($"/guilds/{guildId}/invites")).Deserialize<List<GuildInvite>>().SetClientsInList(client);
        }

        /// <summary>
        /// Gets a guild's invites
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static IReadOnlyList<GuildInvite> GetGuildInvites(this DiscordClient client, ulong guildId)
        {
            return client.GetGuildInvitesAsync(guildId).GetAwaiter().GetResult();
        }
    }
}
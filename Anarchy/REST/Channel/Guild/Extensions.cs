using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord
{
    public static class GuildChannelExtensions
    {
#pragma warning disable IDE1006
        public static async Task<IReadOnlyList<GuildChannel>> GetGuildChannelsAsync(this DiscordClient client, ulong guildId)
        {
            var channels = (await client.HttpClient.GetAsync($"/guilds/{guildId}/channels"))
                                        .DeserializeExArray<GuildChannel>().SetClientsInList(client);

            foreach (var channel in channels)
                channel.GuildId = guildId;

            return channels;
        }

        /// <summary>
        /// Gets a guild's channels
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static IReadOnlyList<GuildChannel> GetGuildChannels(this DiscordClient client, ulong guildId)
        {
            return client.GetGuildChannelsAsync(guildId).Result;
        }


        public static async Task<GuildChannel> CreateGuildChannelAsync(this DiscordClient client, ulong guildId, string name, ChannelType type, ulong? parentId = null)
        {
            var channel = (await client.HttpClient.PostAsync($"/guilds/{guildId}/channels", new GuildChannelCreationProperties() { Name = name, Type = type, ParentId = parentId }))
                                .DeserializeEx<GuildChannel>().SetClient(client);

            channel.GuildId = guildId;

            return channel;
        }

        /// <summary>
        /// Creates a guild channel
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <returns>The created <see cref="GuildChannel"/></returns>
        public static GuildChannel CreateGuildChannel(this DiscordClient client, ulong guildId, string name, ChannelType type, ulong? parentId = null)
        {
            return client.CreateGuildChannelAsync(guildId, name, type, parentId).Result;
        }


        public static async Task<DiscordPermissionOverwrite> AddPermissionOverwriteAsync(this DiscordClient client, ulong channelId, ulong affectedId, PermissionOverwriteType type, DiscordPermission allow, DiscordPermission deny)
        {
            var overwrite = new DiscordPermissionOverwrite() { AffectedId = affectedId, Type = type, Allow = allow, Deny = deny };

            await client.HttpClient.PutAsync($"/channels/{channelId}/permissions/{overwrite.AffectedId}", overwrite);

            return overwrite;
        }

        /// <summary>
        /// Adds/edits a permission overwrite to a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="overwrite">The permission overwrite to add/edit</param>
        public static DiscordPermissionOverwrite AddPermissionOverwrite(this DiscordClient client, ulong channelId, ulong affectedId, PermissionOverwriteType type, DiscordPermission allow, DiscordPermission deny)
        {
            return client.AddPermissionOverwriteAsync(channelId, affectedId, type, allow, deny).Result;
        }


        public static async Task RemovePermissionOverwriteAsync(this DiscordClient client, ulong channelId, ulong affectedId)
        {
            await client.HttpClient.DeleteAsync($"/channels/{channelId}/permissions/{affectedId}");
        }

        /// <summary>
        /// Removes a permission overwrite from a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="id">ID of the role or member affected by the overwrite</param>
        public static void RemovePermissionOverwrite(this DiscordClient client, ulong channelId, ulong affectedId)
        {
            client.RemovePermissionOverwriteAsync(channelId, affectedId).GetAwaiter().GetResult();
        }
#pragma warning restore IDE1006
    }
}

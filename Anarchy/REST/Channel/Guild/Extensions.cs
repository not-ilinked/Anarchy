using System.Collections.Generic;

namespace Discord
{
    public static class GuildChannelExtensions
    {
#pragma warning disable IDE1006
        /// <summary>
        /// Gets a guild's channels
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static IReadOnlyList<GuildChannel> GetGuildChannels(this DiscordClient client, ulong guildId)
        {
            var channels = client.HttpClient.Get($"/guilds/{guildId}/channels")
                                        .DeserializeExArray<GuildChannel>().SetClientsInList(client);

            foreach (var channel in channels)
                channel.GuildId = guildId;

            return channels;
        }


        /// <summary>
        /// Creates a guild channel
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <returns>The created <see cref="GuildChannel"/></returns>
        public static GuildChannel CreateGuildChannel(this DiscordClient client, ulong guildId, string name, ChannelType type, ulong? parentId = null)
        {
            var channel = client.HttpClient.Post($"/guilds/{guildId}/channels", new GuildChannelCreationProperties() { Name = name, Type = type, ParentId = parentId })
                                .DeserializeEx<GuildChannel>().SetClient(client);

            channel.GuildId = guildId;

            return channel;
        }


        /// <summary>
        /// Adds/edits a permission overwrite to a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="overwrite">The permission overwrite to add/edit</param>
        public static DiscordPermissionOverwrite AddPermissionOverwrite(this DiscordClient client, ulong channelId, ulong affectedId, PermissionOverwriteType type, DiscordPermission allow, DiscordPermission deny)
        {
            var overwrite = new DiscordPermissionOverwrite() { AffectedId = affectedId, Type = type, Allow = allow, Deny = deny };

            client.HttpClient.Put($"/channels/{channelId}/permissions/{overwrite.AffectedId}", overwrite);

            return overwrite;
        }


        /// <summary>
        /// Removes a permission overwrite from a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="id">ID of the role or member affected by the overwrite</param>
        public static void RemovePermissionOverwrite(this DiscordClient client, ulong channelId, ulong affectedId)
        {
            client.HttpClient.Delete($"/channels/{channelId}/permissions/{affectedId}");
        }
#pragma warning restore IDE1006
    }
}

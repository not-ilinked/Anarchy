using System.Collections.Generic;

namespace Discord
{
    public static class GuildChannelExtensions
    {
#pragma warning disable IDE1006
        private static Treturn modifyChannel<Treturn, TProperties>(this DiscordClient client, ulong channelId, TProperties properties) where TProperties : GuildChannelProperties where Treturn : GuildChannel
        {
            return client.HttpClient.Patch($"/channels/{channelId}", properties).DeserializeEx<Treturn>().SetClient(client);
        }


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
        /// Modifies a guild channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="properties">Options for modifying the guild channel</param>
        /// <returns>The modified <see cref="GuildChannel"/></returns>
        public static GuildChannel ModifyGuildChannel(this DiscordClient client, ulong channelId, GuildChannelProperties properties)
        {
            return client.modifyChannel<GuildChannel, GuildChannelProperties>(channelId, properties);
        }


        /// <summary>
        /// Modifies a guild text channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="properties">Options for modifying the channel</param>
        /// <returns>The modified <see cref="TextChannel"/></returns>
        public static TextChannel ModifyTextChannel(this DiscordClient client, ulong channelId, TextChannelProperties properties)
        {
            return client.modifyChannel<TextChannel, TextChannelProperties>(channelId, properties);
        }


        /// <summary>
        /// Modifies a guild voice channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="properties">Options for modifying the channel</param>
        /// <returns>The modified <see cref="VoiceChannel"/></returns>
        public static VoiceChannel ModifyVoiceChannel(this DiscordClient client, ulong channelId, VoiceChannelProperties properties)
        {
            return client.modifyChannel<VoiceChannel, VoiceChannelProperties>(channelId, properties);
        }


        /// <summary>
        /// Adds/edits a permission overwrite to a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="overwrite">The permission overwrite to add/edit</param>
        public static void AddPermissionOverwrite(this DiscordClient client, ulong channelId, DiscordPermissionOverwrite overwrite)
        {
            client.HttpClient.Put($"/channels/{channelId}/permissions/{overwrite.Id}", overwrite);
        }


        /// <summary>
        /// Removes a permission overwrite from a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="id">ID of the role or member affected by the overwrite</param>
        public static void RemovePermissionOverwrite(this DiscordClient client, ulong channelId, ulong id)
        {
            client.HttpClient.Delete($"/channels/{channelId}/permissions/{id}");
        }
#pragma warning restore IDE1006
    }
}

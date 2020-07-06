using Newtonsoft.Json;

namespace Discord
{
    public static class ChannelExtensions
    {
        /// <summary>
        /// Gets a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        public static DiscordChannel GetChannel(this DiscordClient client, ulong channelId)
        {
            return client.HttpClient.Get($"/channels/{channelId}")
                                .DeserializeEx<DiscordChannel>().SetClient(client);
        }


        public static DiscordGroup ModifyGroup(this DiscordClient client, ulong groupId, GroupProperties properties)
        {
            return client.HttpClient.Patch($"/channels/{groupId}", properties).DeserializeEx<DiscordGroup>().SetClient(client);
        }


        public static GuildChannel ModifyGuildChannel(this DiscordClient client, ulong channelId, GuildChannelProperties properties)
        {
            return client.HttpClient.Patch($"/channels/{channelId}", properties).DeserializeEx<GuildChannel>().SetClient(client);
        }


        /// <summary>
        /// Deletes a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <returns>The deleted channel</returns>
        public static DiscordChannel DeleteChannel(this DiscordClient client, ulong channelId)
        {
            return client.HttpClient.Delete($"/channels/{channelId}")
                                .DeserializeEx<DiscordChannel>().SetClient(client);
        }
    }
}

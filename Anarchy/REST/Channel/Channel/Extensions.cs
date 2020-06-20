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


        /// <summary>
        /// Modifies a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="properties">Options for modifying the channel</param>
        /// <returns>The modified <see cref="DiscordChannel"/></returns>
        public static DiscordChannel ModifyChannel(this DiscordClient client, ulong channelId, ChannelProperties properties)
        {
            return client.HttpClient.Patch($"/channels/{channelId}", properties).DeserializeEx<DiscordChannel>().SetClient(client);
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

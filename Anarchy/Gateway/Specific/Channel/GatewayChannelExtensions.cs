using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord.Gateway
{
    public static class GatewayChannelExtensions
    {
        public static async Task<DiscordChannel> GetChannelAsync(this DiscordSocketClient client, ulong channelId)
        {
            if (client.Config.Cache)
            {
                foreach (var guild in client.GetCachedGuilds())
                {
                    foreach (var channel in guild.Channels)
                    {
                        if (channel.Id == channelId)
                            return channel;
                    }
                }

                foreach (var channel in client.PrivateChannels)
                {
                    if (channel.Id == channelId)
                        return channel;
                }

                throw new DiscordHttpException(client, new DiscordHttpError(DiscordError.UnknownChannel, "Channel was not found in cache"));
            }
            else
                return await ((DiscordClient)client).GetChannelAsync(channelId);
        }

        /// <summary>
        /// Gets a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        public static DiscordChannel GetChannel(this DiscordSocketClient client, ulong channelId)
        {
            return client.GetChannelAsync(channelId).GetAwaiter().GetResult();
        }


        public static async Task<IReadOnlyList<GuildChannel>> GetGuildChannelsAsync(this DiscordSocketClient client, ulong guildId)
        {
            if (client.Config.Cache)
                return client.GetCachedGuild(guildId).Channels;
            else
                return await ((DiscordClient)client).GetGuildChannelsAsync(guildId);
        }

        /// <summary>
        /// Gets a guild's channels
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static IReadOnlyList<GuildChannel> GetGuildChannels(this DiscordSocketClient client, ulong guildId)
        {
            return client.GetGuildChannelsAsync(guildId).GetAwaiter().GetResult();
        }


        public static async Task<IReadOnlyList<DiscordMessage>> GetChannelMessagesAsync(this DiscordSocketClient client, ulong channelId, MessageFilters filters = null)
        {
            var messages = await ((DiscordClient)client).GetChannelMessagesAsync(channelId, filters);

            if (client.Config.Cache)
            {
                DiscordChannel channel = client.GetChannel(channelId);

                if (channel.Type == ChannelType.Text || channel.Type == ChannelType.Voice || channel.Type == ChannelType.Category)
                {
                    GuildChannel guildChannel = channel.ToGuildChannel();

                    foreach (var message in messages)
                        message.GuildId = guildChannel.GuildId;
                }
            }

            return messages;
        }

        /// <summary>
        /// Gets a list of messages from a channel.
        /// The list is ordered first -> last.
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="filters">Options for filtering out messages</param>
        public static IReadOnlyList<DiscordMessage> GetChannelMessages(this DiscordSocketClient client, ulong channelId, MessageFilters filters = null)
        {
            return client.GetChannelMessagesAsync(channelId, filters).GetAwaiter().GetResult();
        }


        public static async Task<IReadOnlyList<DiscordMessage>> GetChannelMessagesAsync(this DiscordSocketClient client, ulong channelId, uint limit)
        {
            return await client.GetChannelMessagesAsync(channelId, new MessageFilters() { Limit = limit });
        }

        /// <summary>
        /// Gets a list of messages from a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="limit">Max amount of messages to receive</param>
        public static IReadOnlyList<DiscordMessage> GetChannelMessages(this DiscordSocketClient client, ulong channelId, uint limit)
        {
            return client.GetChannelMessagesAsync(channelId, limit).GetAwaiter().GetResult();
        }
    }
}

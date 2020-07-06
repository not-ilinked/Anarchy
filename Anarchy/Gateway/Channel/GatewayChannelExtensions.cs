using System;
using System.Collections.Generic;
using System.Linq;

namespace Discord.Gateway
{
    public static class GatewayChannelExtensions
    {
        /// <summary>
        /// Gets a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        public static DiscordChannel GetChannel(this DiscordSocketClient client, ulong channelId)
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
                return ((DiscordClient)client).GetChannel(channelId);
        }


        /// <summary>
        /// Gets a guild's channels
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static IReadOnlyList<GuildChannel> GetGuildChannels(this DiscordSocketClient client, ulong guildId)
        {
            if (client.Config.Cache)
                return client.GetCachedGuild(guildId).Channels;
            else
                return ((DiscordClient)client).GetGuildChannels(guildId);
        }


        /// <summary>
        /// Gets a list of messages from a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="filters">Options for filtering out messages</param>
        public static IReadOnlyList<DiscordMessage> GetChannelMessages(this DiscordSocketClient client, ulong channelId, MessageFilters filters = null)
        {
            var messages = ((DiscordClient)client).GetChannelMessages(channelId, filters);

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
        /// Gets a list of messages from a channel
        /// </summary>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="limit">Max amount of messages to receive</param>
        public static IReadOnlyList<DiscordMessage> GetChannelMessages(this DiscordSocketClient client, ulong channelId, uint limit)
        {
            return client.GetChannelMessages(channelId, new MessageFilters() { Limit = limit });
        }
    }
}

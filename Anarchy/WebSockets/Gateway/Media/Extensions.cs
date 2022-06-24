using System;
using System.Collections.Generic;
using Discord.Media;

namespace Discord.Gateway
{
    public static class MediaExtensions
    {
        /// <summary>
        /// Changes a client's voice state
        /// </summary>
        public static void ChangeVoiceState(this DiscordSocketClient client, VoiceStateProperties properties)
        {
            client.Send(GatewayOpcode.VoiceStateUpdate, properties.Fill(client));
        }


        public static DiscordVoiceStateContainer GetVoiceStates(this DiscordSocketClient client, ulong userId)
        {
            if (!client.Config.Cache)
                throw new NotSupportedException("Caching is disabled for this client.");

            return client.VoiceStates[userId, true];
        }


        public static IReadOnlyList<DiscordVoiceState> GetGuildVoiceStates(this DiscordSocketClient client, ulong guildId)
        {
            if (!client.Config.Cache)
                throw new NotSupportedException("Caching is disabled for this client.");

            return client.GetCachedGuild(guildId).VoiceStates;
        }


        public static IReadOnlyList<DiscordVoiceState> GetChannelVoiceStates(this DiscordSocketClient client, ulong channelId)
        {
            if (!client.Config.Cache)
                throw new NotSupportedException("Caching is disabled for this client.");

            client.GetChannel(channelId);

            List<DiscordVoiceState> states = new List<DiscordVoiceState>();
            lock (client.VoiceStates.Lock)
            {
                foreach (var state in client.VoiceStates.Values)
                {
                    if (state.PrivateChannelVoiceState != null && state.PrivateChannelVoiceState.Channel.Id == channelId)
                        states.Add(state.PrivateChannelVoiceState);
                    else
                    {
                        foreach (var guildState in state.GuildStates.Values)
                        {
                            if (guildState.Channel != null && guildState.Channel.Id == channelId)
                            {
                                states.Add(guildState);

                                break;
                            }
                        }
                    }
                }
            }

            return states;
        }


        internal static void EndGoLive(this DiscordSocketClient client, string streamKey)
        {
            client.Send(GatewayOpcode.EndGoLive, new GoLiveStreamKey() { StreamKey = streamKey });
        }
    }
}

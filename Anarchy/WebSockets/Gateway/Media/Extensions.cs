using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            if (client.Config.Cache)
            {
                DiscordVoiceStateContainer states;

                try
                {
                    states = client.GetVoiceStates(client.User.Id);
                }
                catch (DiscordHttpException ex)
                {
                    if (ex.Code == DiscordError.UnknownUser)
                        states = new DiscordVoiceStateContainer(client.User.Id);
                    else
                        throw;
                }

                DiscordVoiceState state = null;
                
                if (properties.GuildProperty.Set)
                {
                    if (properties.GuildId.HasValue)
                        states.GuildVoiceStates.TryGetValue(properties.GuildId.Value, out state);
                    else
                        state = states.PrivateChannelVoiceState;

                    if (state != null && !properties.ChannelProperty.Set)
                        properties.ChannelId = state.Channel == null ? null : (ulong?)state.Channel.Id;
                }
                else if (properties.ChannelProperty.Set)
                {
                    var channel = client.GetChannel(properties.ChannelId.Value);

                    if (channel.Type != ChannelType.Group && channel.Type != ChannelType.DM)
                    {
                        properties.GuildId = ((GuildChannel)channel).GuildId;

                        if (states.GuildVoiceStates.TryGetValue(properties.GuildId.Value, out DiscordVoiceState oldState))
                            state = oldState;
                    }
                    else if (states.PrivateChannelVoiceState != null && states.PrivateChannelVoiceState.Channel != null && states.PrivateChannelVoiceState.Channel.Id == properties.ChannelId)
                        state = states.PrivateChannelVoiceState;
                }

                if (state != null)
                {
                    if (!properties.DeafProperty.Set)
                        properties.Deafened = state.SelfDeafened;

                    if (!properties.MutedProperty.Set)
                        properties.Muted = state.Muted;

                    if (!properties.VideoProperty.Set)
                        properties.Video = state.Video;
                }
            }

            client.Send(GatewayOpcode.VoiceStateUpdate, properties);
        }


        public static Task<DiscordVoiceSession> JoinVoiceChannelAsync(this DiscordSocketClient client, VoiceStateProperties properties)
        {
            if (!properties.ChannelProperty.Set || !properties.ChannelId.HasValue)
                throw new ArgumentNullException("ChannelId can not be null");

            client.VoiceSessions.AllowNewConnection(properties.GuildId);

            CancellationTokenSource source = new CancellationTokenSource();
            TaskCompletionSource<DiscordVoiceSession> task = new TaskCompletionSource<DiscordVoiceSession>();

            void serverHandler(DiscordMediaSession session, DiscordMediaServer server)
            {
                source.Cancel();

                if (server.Endpoint != null)
                {
                    session.OnServerUpdated -= serverHandler;

                    if (task.Task.Status != TaskStatus.Faulted)
                        task.SetResult((DiscordVoiceSession)session);
                }
            }

            void stateHandler(DiscordSocketClient c, DiscordVoiceState state)
            {
                if (state.UserId == c.User.Id && state.Channel != null && state.Channel.Id == properties.ChannelId.Value)
                {
                    client.OnConnectionVoiceState -= stateHandler;

                    client.VoiceSessions.CreateSession(properties.GuildId, properties.ChannelId.Value).OnServerUpdated += serverHandler;
                }
            }

            client.OnConnectionVoiceState += stateHandler;

            client.ChangeVoiceState(properties);

            if (client.Config.VoiceChannelConnectTimeout > 0)
            {
                Task.Run(async () =>
                {
                     await Task.Delay(client.Config.VoiceChannelConnectTimeout, source.Token);

                    task.SetException(new TimeoutException("Gateway did not respond with a server"));
                });
            }

            return task.Task;
        }

        /// <summary>
        /// Joins a voice channel.
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="muted">Whether the client will be muted or not</param>
        /// <param name="deafened">Whether the client will be deafened or not</param>
        public static DiscordVoiceSession JoinVoiceChannel(this DiscordSocketClient client, VoiceStateProperties properties)
        {
            return client.JoinVoiceChannelAsync(properties).GetAwaiter().GetResult();
        }


        public static DiscordVoiceStateContainer GetVoiceStates(this DiscordSocketClient client, ulong userId)
        {
            if (!client.Config.Cache)
                throw new NotSupportedException("Caching is disabled for this client.");

            try
            {
                return client.VoiceStates[userId, true];
            }
            catch (KeyNotFoundException)
            {
                throw new DiscordHttpException(client, new DiscordHttpError(DiscordError.UnknownUser, "User's voice state was not found in cache"));
            }
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
                        foreach (var guildState in state.GuildVoiceStates.Values)
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

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
            client.Send(GatewayOpcode.VoiceStateUpdate, properties.Fill(client));
        }


        public static Task<DiscordVoiceSession> JoinVoiceChannelAsync(this DiscordSocketClient client, VoiceStateProperties properties)
        {
            if (!properties.ChannelProperty.Set || !properties.ChannelId.HasValue)
                throw new ArgumentNullException("ChannelId can not be null");

            properties.Fill(client);

            ulong guildId = properties.GuildId ?? 0;

            if (client.VoiceSessions.TryGetValue(guildId, out var oldSession))
                oldSession.Disconnect();

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
                if (state.Channel != null && state.Channel.Id == properties.ChannelId.Value)
                {
                    client.OnSessionVoiceState -= stateHandler;

                    DiscordVoiceSession session = client.VoiceSessions[guildId] = new DiscordVoiceSession(client, properties.GuildId, properties.ChannelId.Value, state.SessionId);
                    session.OnDisconnected += (s, args) => client.VoiceSessions.Remove(guildId);
                    session.OnServerUpdated += serverHandler;
                }
            }

            client.OnSessionVoiceState += stateHandler;

            client.ChangeVoiceState(properties);

            if (client.Config.VoiceChannelConnectTimeout > 0)
            {
                Task.Run(async () =>
                {
                    await Task.Delay((int)client.Config.VoiceChannelConnectTimeout, source.Token);

                    if (!source.IsCancellationRequested)
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

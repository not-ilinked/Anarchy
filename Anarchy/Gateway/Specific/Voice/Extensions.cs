using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Media;
using Discord.Voice;

namespace Discord.Gateway
{
    public static class VoiceExtensions
    {
        /// <summary>
        /// Changes a client's voice state
        /// </summary>
        public static void ChangeVoiceState(this DiscordSocketClient client, VoiceStateChange state)
        {
            // temporary fix for discord expecting all parameters to be set
            if (client.User.Type == DiscordUserType.User && client.Config.Cache)
            {
                try
                {
                    var voiceState = client.GetVoiceState(client.User.Id);

                    if (!state.ChannelProperty.Set && voiceState.Channel != null)
                    {
                        if (voiceState.Guild != null)
                            state.GuildId = voiceState.Guild.Id;

                        state.ChannelId = voiceState.Channel.Id;
                    }

                    if (!state.MutedProperty.Set)
                        state.Muted = voiceState.SelfMuted;

                    if (!state.DeafProperty.Set)
                        state.Deafened = voiceState.SelfDeafened;

                    if (!state.VideoProperty.Set)
                        state.Screensharing = voiceState.Screensharing;
                }
                catch { }
            }

            client.Send(GatewayOpcode.VoiceStateUpdate, state);
        }


        public static Task<DiscordVoiceSession> JoinVoiceChannelAsync(this DiscordSocketClient client, ulong? guildId, ulong channelId, bool muted = false, bool deafened = false)
        {
            // issue with the current code is that it doesn't remove unused sessions
            foreach (var session in client.VoiceSessions)
            {
                if (session.State == DiscordMediaClientState.Connected)
                {
                    if (client.User.Type == DiscordUserType.User || (guildId.HasValue && session.Server.Guild.Id == guildId.Value))
                        session.Disconnect();
                }
            }

            TaskCompletionSource<DiscordVoiceSession> task = new TaskCompletionSource<DiscordVoiceSession>();

            void handler(DiscordSocketClient c, DiscordMediaServer server)
            {
                if (client.User.Type == DiscordUserType.User || (server.Guild != null && guildId.HasValue && server.Guild.Id == guildId.Value))
                {
                    DiscordVoiceSession session = new DiscordVoiceSession(client, server, channelId);

                    client.VoiceSessions.Add(session);

                    client.OnMediaServer -= handler;

                    task.SetResult(session);
                }
            }

            client.OnMediaServer += handler;

            client.ChangeVoiceState(new VoiceStateChange() { GuildId = guildId, ChannelId = channelId, Muted = muted, Deafened = deafened });

            return task.Task;
        }

        /// <summary>
        /// Joins a voice channel.
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="muted">Whether the client will be muted or not</param>
        /// <param name="deafened">Whether the client will be deafened or not</param>
        public static DiscordVoiceSession JoinVoiceChannel(this DiscordSocketClient client, ulong? guildId, ulong channelId, bool muted = false, bool deafened = false)
        {
            return client.JoinVoiceChannelAsync(guildId, channelId, muted, deafened).GetAwaiter().GetResult();
        }


        public static DiscordVoiceState GetVoiceState(this DiscordSocketClient client, ulong userId)
        {
            if (!client.Config.Cache)
                throw new NotSupportedException("Caching is disabled for this client.");

            try
            {
                return client.VoiceStates[userId];
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

            return client.VoiceStates.Values.Where(s => s.Channel != null && s.Channel.Id == channelId).ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            client.Send(GatewayOpcode.VoiceStateUpdate, state);
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
            if (client.Config.ConnectToVoiceChannels)
            {
                VoiceSessionInfo info = null;

                if (client.User.Type == DiscordUserType.User)
                {
                    foreach (var voiceSession in client.VoiceSessions)
                        voiceSession.Session.Disconnect();
                }
                else
                {
                    try
                    {
                        info = client.VoiceSessions.First(s => guildId.HasValue && s.Id == guildId.Value || s.Id == channelId);

                        info.Session.Disconnect();
                    }
                    catch { }
                }

                DiscordVoiceServer server = null;

                client.OnVoiceServer += (c, result) =>
                {
                    server = result;
                };

                client.ChangeVoiceState(new VoiceStateChange() { GuildId = guildId, ChannelId = channelId, Muted = muted, Deafened = deafened });

                int attempts = 0;

                while (server == null)
                {
                    if (attempts >= 300)
                        throw new TimeoutException("Gateway did not respond with a server");

                    Thread.Sleep(10);

                    attempts++;
                }

                DiscordVoiceSession session = new DiscordVoiceSession(client, server, channelId);

                if (info == null)
                    client.VoiceSessions.Add(new VoiceSessionInfo(session, guildId ?? channelId));
                else
                {
                    info.Session = session;
                    info.Id = guildId ?? channelId;
                }

                return session;
            }
            else
            {
                client.ChangeVoiceState(new VoiceStateChange() { GuildId = guildId, ChannelId = channelId, Muted = muted, Deafened = deafened });

                return null;
            }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discord.Gateway
{
    public static class GuildMemberExtensions
    {
        public static IReadOnlyList<SocketGuild> GetCachedGuilds(this DiscordSocketClient client)
        {
            if (!client.Config.Cache)
                throw new NotSupportedException("Caching is disabled for this client.");

            lock (client.GuildCache.Lock)
                return client.GuildCache.Values.ToList();
        }


        public static SocketGuild GetCachedGuild(this DiscordSocketClient client, ulong guildId)
        {
            if (!client.Config.Cache)
                throw new NotSupportedException("Caching is disabled for this client.");

            try
            {
                return client.GuildCache[guildId];
            }
            catch (KeyNotFoundException)
            {
                throw new DiscordHttpException(new DiscordHttpError(DiscordError.UnknownGuild, "Guild was not found in the cache"));
            }
        }


        public static ClientGuildSettings GetGuildSettings(this DiscordSocketClient client, ulong guildId)
        {
            client.GetCachedGuild(guildId);

            try
            {
                return client.GuildSettings[guildId];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }


        public static DiscordChannelSettings GetChannelSettings(this DiscordSocketClient client, ulong channelId)
        {
            foreach (var settings in client.PrivateChannelSettings)
            {
                if (settings.Id == channelId)
                    return settings;
            }

            foreach (var guildSettings in client.GuildSettings.Values)
            {
                foreach (var channel in guildSettings.ChannelOverrides)
                {
                    if (channel.Id == channelId)
                        return channel;
                }
            }

            return null;
        }


        public static Task<IReadOnlyList<GuildMember>> GetGuildMembersAsync(this DiscordSocketClient client, ulong guildId, uint limit = 0)
        {
            List<GuildMember> members = new List<GuildMember>();
            TaskCompletionSource<IReadOnlyList<GuildMember>> task = new TaskCompletionSource<IReadOnlyList<GuildMember>>();

            void handler(DiscordSocketClient c, GuildMembersEventArgs args)
            {
                if (args.GuildId == guildId)
                {
                    members.AddRange(args.Members);

                    if (args.Index + 1 == args.Total)
                    {
                        client.OnGuildMembersReceived -= handler;

                        task.SetResult(members);
                    }
                }
            };

            client.OnGuildMembersReceived += handler;

            client.Send(GatewayOpcode.RequestGuildMembers, new GuildMemberQuery() { GuildId = guildId, Limit = limit });

            return task.Task;
        }

        public static IReadOnlyList<GuildMember> GetGuildMembers(this DiscordSocketClient client, ulong guildId, uint limit = 0)
        {
            return client.GetGuildMembersAsync(guildId, limit).GetAwaiter().GetResult();
        }


        private static void SetGuildSubscriptions(this DiscordSocketClient client, ulong guildId, GuildSubscriptionProperties properties)
        {
            properties.GuildId = guildId;
            client.Send(GatewayOpcode.GuildSubscriptions, properties);
        }


        private static int[][] CreateChunks(int from, bool more)
        {
            int[][] results = new int[more ? 3 : 1][];

            results[0] = new int[] { 0, 99 };

            if (more)
            {
                for (int i = 1; i <= 2; i++)
                {
                    results[i] = new int[] { from, from + 99 };
                    from += 100;
                }
            }

            return results;
        }


        public static void SubscribeToGuildEvents(this DiscordSocketClient client, ulong guildId) => SetGuildSubscriptions(client, guildId, new GuildSubscriptionProperties() { Typing = true, Activities = true, Threads = true });


        public static Task<IReadOnlyList<GuildMember>> GetGuildChannelMembersAsync(this DiscordSocketClient client, ulong guildId, ulong channelId, uint limit = 0)
        {
            var completionSource = new TaskCompletionSource<IReadOnlyList<GuildMember>>();
            List<GuildMember> members = new List<GuildMember>();

            // maybe could've made it start from the last chunk it received,
            // but due to them possibly being logged out for an extended period of time, starting over is better
            void loginHandler(DiscordSocketClient c, LoginEventArgs e)
            {
                members.Clear();
                client.SetGuildSubscriptions(guildId, new GuildSubscriptionProperties()
                {
                    Activities = true,
                    Typing = true,
                    Threads = true,
                    Channels = { { channelId, CreateChunks(0, false) } }
                });
            }

            void handler(DiscordSocketClient s, DiscordMemberListUpdate e)
            {
                if (e.Guild.Id == guildId)
                {
                    int membersAccordingToRoles = 0;

                    foreach (var group in e.Groups)
                        membersAccordingToRoles += group.Count;

                    try
                    {
                        var syncOps = e.Operations.Where(o => o.Type == "SYNC").ToList();

                        for (int i = syncOps.Count - 1; i >= 0; i--)
                        {
                            var operation = syncOps[i];

                            if (operation.Type == "SYNC")
                            {
                                members.AddRange(operation.Items.Select(item => item.Member).Where(m => m != null));

                                if (operation.Items.Count < 100 || (limit > 0 && members.Count >= limit))
                                {
                                    client.OnMemberListUpdate -= handler;
                                    client.OnLoggedIn -= loginHandler;
                                    completionSource.SetResult(limit > 0 ? members.Take((int)limit).ToList() : members);
                                    break;
                                }
                                else if (i == 0)
                                {
                                    if (members.Count == 100)
                                        client.SetGuildSubscriptions(guildId, new GuildSubscriptionProperties() { Channels = { { channelId, CreateChunks(0, false) } } });

                                    client.SetGuildSubscriptions(guildId, new GuildSubscriptionProperties() { Channels = { { channelId, CreateChunks(operation.Range[1] + 1, true) } } });
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        completionSource.SetException(ex);
                    }
                }
            }

            client.OnMemberListUpdate += handler;
            client.OnLoggedIn += loginHandler;

            try
            {
                client.SetGuildSubscriptions(guildId, new GuildSubscriptionProperties()
                {
                    Activities = true,
                    Typing = true,
                    Threads = true,
                    Channels = { { channelId, CreateChunks(0, false) } }
                });
            }
            catch (Exception ex)
            {
                completionSource.SetException(ex);
            }

            return completionSource.Task;
        }

        public static IReadOnlyList<GuildMember> GetGuildChannelMembers(this DiscordSocketClient client, ulong guildId, ulong channelId, uint limit = 0) => client.GetGuildChannelMembersAsync(guildId, channelId, limit).GetAwaiter().GetResult();
    }
}

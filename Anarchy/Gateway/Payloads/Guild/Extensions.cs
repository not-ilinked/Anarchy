using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Discord.Gateway
{
    public static class GuildMemberExtensions
    {
        public static IReadOnlyList<SocketGuild> GetCachedGuilds(this DiscordSocketClient client)
        {
            if (!client.Config.Cache)
                throw new NotSupportedException("Caching is disabled for this client.");

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
                throw new DiscordHttpException(client, new DiscordHttpError(DiscordError.UnknownGuild, "Guild was not found in the cache"));
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


        public static IReadOnlyList<GuildMember> GetGuildMembers(this DiscordSocketClient client, ulong guildId, uint limit = 0)
        {
            List<GuildMember> members = new List<GuildMember>();
            bool done = false;

            void handler(DiscordSocketClient c, GuildMembersEventArgs args)
            {
                if (args.GuildId == guildId)
                {
                    members.AddRange(args.Members);

                    if (args.Index + 1 == args.Total)
                        done = true;
                }
            };

            client.OnGuildMembersReceived += handler;

            client.Send(GatewayOpcode.RequestGuildMembers, new GuildMemberQuery() { GuildId = guildId, Limit = limit });

            while (!done && client.LoggedIn) { Thread.Sleep(10); }

            client.OnGuildMembersReceived -= handler;

            return members;
        }


        private static int RequestMembers(DiscordSocketClient client, ulong guildId, ulong channelId, int index)
        {
            const int rangesPerRequest = 3;

            int[][] chunks = new int[rangesPerRequest][];

            for (int i = 0; i < chunks.Length; i++)
            {
                chunks[i] = new int[] { index, index + 99 };

                index += 100;
            }

            client.Send(GatewayOpcode.RequestGuildMembersUser, new MemberListQuery()
            {
                GuildId = guildId,
                Channels = new Dictionary<ulong, int[][]>() { { channelId, chunks } }
            });

            return rangesPerRequest;
        }


        /// <summary>
        /// Warning: this does not work for official guilds
        /// </summary>
        public static IReadOnlyList<GuildMember> GetGuildChannelMembers(this DiscordSocketClient client, ulong guildId, ulong channelId, MemberListQueryOptions options = null)
        {
            if (options == null)
                options = new MemberListQueryOptions();

            Dictionary<int, GuildMember> memberDict = new Dictionary<int, GuildMember>(); // might as well be a List right now, but this makes it easier to add more operations later
            bool done = false;
            int pendingRequests = 0;

            void handler(DiscordSocketClient c, GuildMemberListEventArgs args)
            {
                if (args.GuildId == guildId)
                {
                    int combined = 0;
                    foreach (var grp in args.Groups)
                        combined += grp.Count;

                    foreach (var op in args.Operations)
                    {
                        if (op["op"].ToString() == "SYNC")
                        {
                            List<GuildMember> newMembers = new List<GuildMember>();

                            foreach (var item in op["items"])
                            {
                                JToken obj = item["member"];

                                if (obj != null)
                                    newMembers.Add(obj.ToObject<GuildMember>());
                            }

                            if (newMembers.Count > 0)
                            {
                                int[] range = op["range"].ToObject<int[]>();

                                for (int i = 0; i < newMembers.Count; i++)
                                    memberDict[i + range[0]] = newMembers[i];

                                pendingRequests--;

                                if ((memberDict.Count >= options.Count && options.Count > 0) || memberDict.OrderBy(i => i.Key).Last().Key + 1 >= combined)
                                    done = true;
                                else if (pendingRequests == 0)
                                    pendingRequests = RequestMembers(client, guildId, channelId, memberDict.OrderBy(i => i.Key).Last().Key);
                            }
                        }
                    }
                }
            }

            client.OnMemberListUpdate += handler;

            pendingRequests = RequestMembers(client, guildId, channelId, options.Index);

            while (!done && client.LoggedIn)
                Thread.Sleep(10);

            client.OnMemberListUpdate -= handler;

            IEnumerable<GuildMember> members = memberDict.Select(i => i.Value);

            if (options.Count > 0)
                members = members.Take(options.Count);

            return members.ToList();
        }
    }
}

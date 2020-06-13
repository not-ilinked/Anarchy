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


        public static ClientGuildSettings GetClientGuildSettings(this DiscordSocketClient client, ulong guildId)
        {
            try
            {
                return client.ClientGuildSettings[guildId];
            }
            catch
            {
                throw new DiscordHttpException(client, new DiscordHttpError(DiscordError.UnknownGuild, "Guild notification settings were not found in the cache"));
            }
        }


        /// <summary>
        /// Requests a member chunk from a guild
        /// </summary>
        public static void RequestGuildMembers(this DiscordSocketClient client, GatewayMemberQuery query)
        {
            client.Send(GatewayOpcode.RequestGuildMembers, query);
        }


        
        /// <summary>
        /// Requests a member chunk from a guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="channelId">ID of the channel</param>
        /// <param name="chunks">Ranges to grab</param>
        public static void RequestGuildMembersNew(this DiscordSocketClient client, ulong guildId, IEnumerable<ulong> channels, int[][] chunks)
        {
            var query = new GatewayUserMemberQuery()
            {
                GuildId = guildId
            };

            foreach (var channel in channels)
                query.Channels.Add(channel, chunks);

            client.Send(GatewayOpcode.RequestGuildMembersUser, query);
        }


        /// <summary>
        /// Gets all memebers in a guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static IReadOnlyList<GuildMember> GetAllGuildMembers(this DiscordSocketClient client, ulong guildId)
        {
            List<GuildMember> members = new List<GuildMember>();

            DateTime lastUpdate = DateTime.Now;
            int ChunkIndex = 0;
            int ChunkCount = 2;

            void lol(DiscordSocketClient c, GuildMembersEventArgs args)
            {
                if (args.GuildId == args.GuildId)
                {
                    lastUpdate = DateTime.Now;
                    ChunkIndex = args.Index.Value;
                    ChunkCount = args.Total;

                    members.AddRange(args.Members);
                }
            }

            client.OnGuildMembersReceived += lol;

            client.RequestGuildMembers(new GatewayMemberQuery() { GuildId = guildId, Limit = 0 });

            while (ChunkIndex < ChunkCount - 1 && DateTime.Now - lastUpdate < new TimeSpan(0, 0, 5)) Thread.Sleep(1);

            client.OnGuildMembersReceived -= lol;

            return members; // for some reason members can sometimes be null
        }


        
        public static IReadOnlyList<GuildMember> GetAllGuildMembersNew(this DiscordSocketClient client, ulong guildId, IEnumerable<ulong> channels)
        {
            List<GuildMember> members = new List<GuildMember>();
            int lastOffset = 100;

            DateTime lastUpdate = DateTime.Now;
            bool done = false;

            void handler(DiscordSocketClient c, GuildMembersEventArgs args)
            {
                if (args.Sync.Value && args.GuildId == guildId)
                {
                    if (args.Members.Count > 0)
                    {
                        members.AddRange(args.Members);

                        int offset = lastOffset;
                        int limit = lastOffset + 99;
                        lastUpdate = DateTime.Now;
                        lastOffset = limit + 1;

                        client.RequestGuildMembersNew(guildId, channels, new int[][] { new int[] { offset, limit } });

                        Console.WriteLine("Another one " + members.Count);
                    }
                    else
                        done = true;
                }
            };

            client.OnGuildMembersReceived += handler;

            client.RequestGuildMembersNew(guildId, channels, new int[][] { new int[] { 0, 99 } });

            while (!done) { Thread.Sleep(1); };

            client.OnGuildMembersReceived -= handler;

            return members.GroupBy(m => m.User.Id).Select(m => m.First()).ToList();
        }
    }
}

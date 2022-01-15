using Discord.Gateway;
using System.Threading.Tasks;

namespace Discord
{
    public static class GuildDiscoveryExtensions
    {
        public static async Task<GuildQueryResult> QueryGuildsAsync(this DiscordClient client, GuildQueryOptions options = null)
        {
            if (options == null)
            {
                options = new GuildQueryOptions();
            }

            string query = $"?limit={options.Limit}&offset={options.Offset}";

            if (options.Query != null)
            {
                query += "&query=" + options.Query;
            }

            if (options.Category.HasValue)
            {
                query += "&categories=" + (int)options.Category;
            }

            return (await client.HttpClient.GetAsync($"/discoverable-guilds" + query)).Deserialize<GuildQueryResult>().SetClient(client);
        }

        /// <summary>
        /// Queries guilds in Server Discovery
        /// </summary>
        public static GuildQueryResult QueryGuilds(this DiscordClient client, GuildQueryOptions options = null)
        {
            return client.QueryGuildsAsync(options).GetAwaiter().GetResult();
        }


        public static async Task<DiscordGuild> LurkGuildAsync(this DiscordSocketClient client, ulong guildId)
        {
            client.Lurking = guildId;

            while (true)
            {
                try
                {
                    return (await client.HttpClient.PutAsync($"/guilds/{guildId}/members/@me?lurker=true&session_id={client.SessionId}"))
                                        .Deserialize<DiscordGuild>().SetClient(client);
                }
                catch (DiscordHttpException ex)
                {
                    if (ex.Code != DiscordError.UnknownSession || client.SessionId == null)
                    {
                        throw;
                    }
                }
            }
        }

        public static DiscordGuild LurkGuild(this DiscordSocketClient client, ulong guildId)
        {
            return client.LurkGuildAsync(guildId).GetAwaiter().GetResult();
        }


        public static async Task<DiscordGuild> JoinGuildAsync(this DiscordClient client, ulong guildId)
        {
            return (await client.HttpClient.PutAsync($"/guilds/{guildId}/members/@me?lurker=false"))
                                .Deserialize<DiscordGuild>().SetClient(client);
        }

        /// <summary>
        /// Joins a lurkable guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <returns></returns>
        public static DiscordGuild JoinGuild(this DiscordClient client, ulong guildId)
        {
            return client.JoinGuildAsync(guildId).GetAwaiter().GetResult();
        }
    }
}

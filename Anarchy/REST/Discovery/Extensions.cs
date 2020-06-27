using Discord.Gateway;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Discord
{
    public static class GuildDiscoveryExtensions
    {
        /// <summary>
        /// Queries guilds in Server Discovery
        /// </summary>
        /// <param name="query">The name to search for</param>
        /// <param name="limit">Max amount of guilds to receive</param>
        /// <param name="offset">The offset in the list</param>
        public static IReadOnlyList<DiscoveryGuild> QueryGuilds(this DiscordClient client, GuildQueryOptions options = null)
        {
            if (options == null)
                options = new GuildQueryOptions();

            List<DiscoveryGuild> guilds = new List<DiscoveryGuild>();

            string query = $"?limit={options.Limit}&offset={options.Offset}";

            if (options.Query != null)
                query += "&query=" + options.Query;

            if (options.Category.HasValue)
                query += "&categories=" + (int)options.Category;


            foreach (var lol in client.HttpClient.Get($"/discoverable-guilds" + query).Deserialize<JObject>().Value<JArray>("guilds"))
                guilds.Add(lol.ToObject<DiscoveryGuild>());

            return guilds.SetClientsInList(client);
        }


        /// <summary>
        /// Activate lurker mode on a guild.
        /// 
        /// Note: this currently does not actually get you into lurk mode on the server because of some weird session_id issues.
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static DiscordGuild LurkGuild(this DiscordSocketClient client, ulong guildId)
        {
            client.Lurking = guildId;

            while (true)
            {
                try
                {
                    return client.HttpClient.Put($"/guilds/{guildId}/members/@me?lurker=true&session_id={client.SessionId}")
                                        .Deserialize<DiscordGuild>().SetClient(client);
                }
                catch (DiscordHttpException ex)
                {
                    if (ex.Code != DiscordError.UnknownSession || client.SessionId == null)
                        throw;
                }
            }
        }


        /// <summary>
        /// Joins a lurkable guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <returns></returns>
        public static DiscordGuild JoinGuild(this DiscordClient client, ulong guildId)
        {
            return client.HttpClient.Put($"/guilds/{guildId}/members/@me?lurker=false")
                                .Deserialize<DiscordGuild>().SetClient(client);
        }
    }
}

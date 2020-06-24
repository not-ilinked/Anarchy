using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;

namespace Discord
{
    public static class GuildExtensions
    {
        #region management
        /// <summary>
        /// Creates a guild
        /// </summary>
        /// <param name="properties">Options for creating the guild</param>
        /// <returns>The created <see cref="DiscordGuild"/></returns>
        public static DiscordGuild CreateGuild(this DiscordClient client, string name, DiscordImage icon = null, string region = null)
        {
            GuildCreationProperties properties = new GuildCreationProperties() { Name = name, Icon = icon, Region = region };

            return client.HttpClient.Post("/guilds", properties).Deserialize<DiscordGuild>().SetClient(client);
        }


        /// <summary>
        /// Modifies a guild
        /// </summary>
        /// <param name="guildId">ID of the group</param>
        /// <param name="properties">Options for modifying the guild</param>
        /// <returns>The modified <see cref="DiscordGuild"/></returns>
        public static DiscordGuild ModifyGuild(this DiscordClient client, ulong guildId, GuildProperties properties)
        {
            if (properties.VanityProperty.Set)
                client.SetGuildVanityUrl(guildId, properties.VanityUrlCode);

            return client.HttpClient.Patch($"/guilds/{guildId}", properties).Deserialize<DiscordGuild>().SetClient(client);
        }


        /// <summary>
        /// Deletes a guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static void DeleteGuild(this DiscordClient client, ulong guildId)
        {
            client.HttpClient.Delete($"/guilds/{guildId}");
        }



        public static void SetGuildVanityUrl(this DiscordClient client, ulong guildId, string vanityCode)
        {
            client.HttpClient.Patch($"/guilds/{guildId}/vanity-url", $"{{\"code\":\"{vanityCode}\"}}");
        }


        /// <summary>
        /// Kicks a member from a guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="userId">ID of the member</param>
        public static void KickGuildMember(this DiscordClient client, ulong guildId, ulong userId)
        {
            client.HttpClient.Delete($"/guilds/{guildId}/members/{userId}");
        }


        /// <summary>
        /// Gets the guild's banned users
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static IReadOnlyList<Ban> GetGuildBans(this DiscordClient client, ulong guildId)
        {
            IReadOnlyList<Ban> bans = client.HttpClient.Get($"/guilds/{guildId}/bans")
                                                    .Deserialize<IReadOnlyList<Ban>>().SetClientsInList(client);
            foreach (var ban in bans)
                ban.GuildId = guildId;
            return bans;
        }


        /// <summary>
        /// Gets a guild's banned user
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="userId">ID of the user</param>
        /// <returns></returns>
        public static Ban GetGuildBan(this DiscordClient client, ulong guildId, ulong userId)
        {
            Ban ban = client.HttpClient.Get($"/guilds/{guildId}/bans/{userId}")
                                   .Deserialize<Ban>().SetClient(client);
            ban.GuildId = guildId;
            return ban;
        }


        /// <summary>
        /// Bans a member from a guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="userId">ID of the member</param>
        /// <param name="reason">Reason for banning the member</param>
        /// <param name="deleteMessageDays">Amount of days to purge messages for</param>
        public static void BanGuildMember(this DiscordClient client, ulong guildId, ulong userId, string reason = null, uint deleteMessageDays = 0)
        {
            client.HttpClient.Put($"/guilds/{guildId}/bans/{userId}?delete-message-days={deleteMessageDays}&reason={reason}");
        }


        /// <summary>
        /// Unbans a user from a guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="userId">ID of the user</param>
        public static void UnbanGuildMember(this DiscordClient client, ulong guildId, ulong userId)
        {
            client.HttpClient.Delete($"/guilds/{guildId}/bans/{userId}");
        }
        #endregion


        public static ClientGuildSettings ModifyGuildSettings(this DiscordClient client, ulong guildId, GuildSettingsProperties properties)
        {
            return client.HttpClient.Patch($"/users/@me/guilds/{guildId}/settings", properties).Deserialize<ClientGuildSettings>().SetClient(client);
        }


        public static IReadOnlyList<DiscordChannelSettings> SetPrivateChannelSettings(this DiscordClient client, Dictionary<ulong, ChannelSettingsProperties> channels)
        {
            JObject container = new JObject
            {
                ["channel_overrides"] = JObject.FromObject(channels)
            };

            return client.HttpClient.Patch($"/users/@me/guilds/@me/settings", container).Deserialize<JObject>()["channel_overrides"].ToObject<List<DiscordChannelSettings>>();
        }


        /// <summary>
        /// Gets the guilds the account is in
        /// </summary>
        /// <param name="limit">Max amount of guild to receive</param>
        /// <param name="afterId">Guild ID to offset from</param>
        public static IReadOnlyList<PartialGuild> GetGuilds(this DiscordClient client, uint limit = 100, ulong afterId = 0)
        {
                return client.HttpClient.Get($"/users/@me/guilds?limit={limit}&after={afterId}")
                                .Deserialize<IReadOnlyList<PartialGuild>>().SetClientsInList(client);
        }


        /// <summary>
        /// Gets a guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static DiscordGuild GetGuild(this DiscordClient client, ulong guildId)
        {
            return client.HttpClient.Get("/guilds/" + guildId)
                                .Deserialize<DiscordGuild>().SetClient(client);
        }


        /// <summary>
        /// Joins a guild
        /// </summary>
        /// <returns>The invite used to join the guild</returns>
        public static GuildInvite JoinGuild(this DiscordClient client, string invCode)
        {
            return client.HttpClient.Post($"/invite/{invCode}")
                                .Deserialize<GuildInvite>().SetClient(client);
        }


        /// <summary>
        /// Leaves a guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static void LeaveGuild(this DiscordClient client, ulong guildId, bool lurking = false)
        {
            client.HttpClient.Delete($"/users/@me/guilds/{guildId}", $"{{\"lurking\":{lurking.ToString().ToLower()}}}");
        }


        /// <summary>
        /// Changes the client's nickname in a guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="nickname">New nickname</param>
        public static void ChangeClientNickname(this DiscordClient client, ulong guildId, string nickname)
        {
            client.HttpClient.Patch($"/guilds/{guildId}/members/@me/nick", $"{{\"nick\":\"{nickname}\"}}");
        }


        /// <summary>
        /// Acknowledges all messages and pings in a guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        public static void AcknowledgeGuildMessages(this DiscordClient client, ulong guildId)
        {
            client.HttpClient.Post($"/guilds/{guildId}/ack");
        }


        public static Image GetGoLivePreview(this DiscordClient client, ulong guildId, ulong channelId, ulong userId)
        {
            return (Bitmap)new ImageConverter().ConvertFrom(new HttpClient().GetByteArrayAsync(client.HttpClient.Get($"https://discordapp.com/api/v6/streams/guild:{guildId}:{channelId}:{userId}/preview?version=1589053944368").Deserialize<JObject>().Value<string>("url")).Result);
        }
    }
}
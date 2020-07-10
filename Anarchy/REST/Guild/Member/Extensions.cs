using System.Threading.Tasks;

namespace Discord
{
    public static class GuildMemberExtensions
    {
        public static async Task<GuildMember> GetGuildMemberAsync(this DiscordClient client, ulong guildId, ulong userId)
        {
            GuildMember member = (await client.HttpClient.GetAsync($"/guilds/{guildId}/members/{userId}"))
                                            .Deserialize<GuildMember>().SetClient(client);
            member.GuildId = guildId;
            return member;
        }

        /// <summary>
        /// Gets a member of a guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="userId">ID of the user</param>
        public static GuildMember GetGuildMember(this DiscordClient client, ulong guildId, ulong userId)
        {
            return client.GetGuildMemberAsync(guildId, userId).GetAwaiter().GetResult();
        }


        public static async Task ModifyGuildMemberAsync(this DiscordClient client, ulong guildId, ulong userId, GuildMemberProperties properties)
        {
            await client.HttpClient.PatchAsync($"/guilds/{guildId}/members/{userId}", properties);
        }

        /// <summary>
        /// Modifies the specified guild member
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="userId">ID of the user</param>
        /// <param name="properties">Things to change</param>
        public static void ModifyGuildMember(this DiscordClient client, ulong guildId, ulong userId, GuildMemberProperties properties)
        {
            client.ModifyGuildMemberAsync(guildId, userId, properties).GetAwaiter().GetResult();
        }


        public static async Task ChangeNicknameAsync(this DiscordClient client, ulong guildId, ulong userId, string nickname)
        {
            await client.ModifyGuildMemberAsync(guildId, userId, new GuildMemberProperties() { Nickname = nickname });
        }

        /// <summary>
        /// Changes a user's nickname in a guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="userId">ID of the user</param>
        /// <param name="nickname">New nickname</param>
        public static void ChangeNickname(this DiscordClient client, ulong guildId, ulong userId, string nickname)
        {
            client.ChangeNicknameAsync(guildId, userId, nickname).GetAwaiter().GetResult();
        }
    }
}

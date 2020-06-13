using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Discord
{
    public static class GuildMemberExtensions
    {
        /// <summary>
        /// Gets a member of a guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="userId">ID of the user</param>
        public static GuildMember GetGuildMember(this DiscordClient client, ulong guildId, ulong userId)
        {
            GuildMember member = client.HttpClient.Get($"/guilds/{guildId}/members/{userId}")
                                            .Deserialize<GuildMember>().SetClient(client);
            member.GuildId = guildId;
            return member;
        }


        /// <summary>
        /// Modifies the specified guild member
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="userId">ID of the user</param>
        /// <param name="properties">Things to change</param>
        public static void ModifyGuildMember(this DiscordClient client, ulong guildId, ulong userId, GuildMemberProperties properties)
        {
            client.HttpClient.Patch($"/guilds/{guildId}/members/{userId}", properties);
        }


        /// <summary>
        /// Changes a user's nickname in a guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="userId">ID of the user</param>
        /// <param name="nickname">New nickname</param>
        public static void ChangeNickname(this DiscordClient client, ulong guildId, ulong userId, string nickname)
        {
            client.ModifyGuildMember(guildId, userId, new GuildMemberProperties() { Nickname = nickname });
        }
    }
}

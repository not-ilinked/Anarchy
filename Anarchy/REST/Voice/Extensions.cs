using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Discord
{
    public static class VoiceExtensions
    {
        /// <summary>
        /// Rings the specified recipients
        /// </summary>
        public static void Ring(this DiscordClient client, ulong channelId, List<ulong> recipients)
        {
            JObject obj = new JObject
            {
                ["recipients"] = recipients == null ? null : JArray.FromObject(recipients)
            };

            client.HttpClient.Post($"/channels/{channelId}/call/ring", obj);
        }


        /// <summary>
        /// Opens a call on the specified channel
        /// </summary>
        public static void StartCall(this DiscordClient client, ulong channelId)
        {
            client.Ring(channelId, null);
        }


        /// <summary>
        /// Stops ringing the specified recipients
        /// </summary>
        public static void StopRinging(this DiscordClient client, ulong channelId, List<ulong> recipients)
        {
            JObject obj = new JObject
            {
                ["recipients"] = recipients == null ? null : JArray.FromObject(recipients)
            };

            client.HttpClient.Post($"https://discordapp.com/api/v6/channels/{channelId}/call/stop-ringing", obj);
        }


        /// <summary>
        /// Declines the current incoming call from the specified channel
        /// </summary>
        public static void DeclineCall(this DiscordClient client, ulong channelId)
        {
            client.StopRinging(channelId, null);
        }


        /// <summary>
        /// Gets all available voice regions
        /// </summary>
        public static IReadOnlyList<VoiceRegion> GetVoiceRegions(this DiscordClient client)
        {
            return client.HttpClient.Get("/voice/regions")
                                .Deserialize<IReadOnlyList<VoiceRegion>>();
        }

        /// <summary>
        /// Mutes the user in the specified guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="userId">ID of the user</param>
        /// <param name="unmute">Unmute the user instead of muting them</param>
        public static void MuteGuildMember(this DiscordClient client, ulong guildId, ulong userId, bool unmute = false)
        {
            client.ModifyGuildMember(guildId, userId, new GuildMemberProperties() { Muted = !unmute });
        }


        /// <summary>
        /// Deafenes the user in the specified guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="userId">ID of the user</param>
        /// <param name="undeafen">Undeafen the user instead of deafening them</param>
        public static void DeafenGuildMember(this DiscordClient client, ulong guildId, ulong userId, bool undeafen = false)
        {
            client.ModifyGuildMember(guildId, userId, new GuildMemberProperties() { Deafened = !undeafen });
        }
    }
}

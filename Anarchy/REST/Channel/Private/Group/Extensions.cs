using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord
{
    public static class GroupExtensions
    {
        public static async Task<DiscordInvite> JoinGroupAsync(this DiscordClient client, string inviteCode)
        {
            return (await client.HttpClient.PostAsync($"/invites/{inviteCode}"))
                                .Deserialize<DiscordInvite>().SetClient(client);
        }

        /// <summary>
        /// Joins a group
        /// </summary>
        /// <param name="inviteCode">Invite for the group</param>
        /// <returns>The invite used</returns>
        public static DiscordInvite JoinGroup(this DiscordClient client, string inviteCode)
        {
            return client.JoinGroupAsync(inviteCode).GetAwaiter().GetResult();
        }


        public static async Task<DiscordGroup> CreateGroupAsync(this DiscordClient client, List<ulong> recipients)
        {
            return (await client.HttpClient.PostAsync($"/users/@me/channels", new JObject()
            {
                ["recipients"] = JArray.FromObject(recipients)
            })).DeserializeEx<DiscordGroup>().SetClient(client);
        }

        /// <summary>
        /// Creates a group
        /// </summary>
        /// <param name="recipients">The IDs of the recipients to add</param>
        /// <returns>The created <see cref="DiscordGroup"/></returns>
        public static DiscordGroup CreateGroup(this DiscordClient client, List<ulong> recipients)
        {
            return client.CreateGroupAsync(recipients).GetAwaiter().GetResult();
        }


        public static async Task<DiscordChannel> LeaveGroupAsync(this DiscordClient client, ulong groupId)
        {
            return await client.DeleteChannelAsync(groupId);
        }

        // This does the same as DeleteChannel(), i just decided to leave it be because DeleteChannel() is a weird name for a function for leaving groups
        /// <summary>
        /// Leaves a group.
        /// </summary>
        /// <param name="groupId">ID of the group</param>
        /// <returns>The leaved <see cref="DiscordGroup"/></returns>
        public static DiscordChannel LeaveGroup(this DiscordClient client, ulong groupId)
        {
            return client.LeaveGroupAsync(groupId).GetAwaiter().GetResult();
        }


        public static async Task AddUserToGroupAsync(this DiscordClient client, ulong groupId, ulong userId)
        {
            await client.HttpClient.PutAsync($"/channels/{groupId}/recipients/{userId}");
        }

        /// <summary>
        /// Adds a user to a group
        /// </summary>
        /// <param name="groupId">ID of the group</param>
        /// <param name="userId">ID of the user</param>
        public static void AddUserToGroup(this DiscordClient client, ulong groupId, ulong userId)
        {
            client.AddUserToGroupAsync(groupId, userId).GetAwaiter().GetResult();
        }


        public static async Task RemoveUserFromGroupAsync(this DiscordClient client, ulong groupId, ulong userId)
        {
            await client.HttpClient.DeleteAsync($"/channels/{groupId}/recipients/{userId}");
        }

        /// <summary>
        /// Removes a user from a group
        /// </summary>
        /// <param name="groupId">ID of the group</param>
        /// <param name="userId">ID of the user</param>
        public static void RemoveUserFromGroup(this DiscordClient client, ulong groupId, ulong userId)
        {
            client.RemoveUserFromGroupAsync(groupId, userId).GetAwaiter().GetResult();
        }
    }
}

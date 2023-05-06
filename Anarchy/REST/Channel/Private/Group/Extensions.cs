using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Discord
{
    public static class GroupExtensions
    {
        public static async Task<DiscordInvite> JoinGroupAsync(this DiscordClient client, string inviteCode)
        {
            return (await client.HttpClient.PostAsync($"/invites/{inviteCode}"))
                                    .ParseDeterministic<DiscordInvite>().SetClient(client);
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
            return (await client.HttpClient.PostAsync($"/users/@me/channels", new JsonObject()
            {
                ["recipients"] = recipients.ToString(),
            })).Deserialize<DiscordGroup>().SetClient(client);
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

        public static async Task<DiscordGroup> LeaveGroupAsync(this DiscordClient client, ulong groupId)
        {
            return (DiscordGroup) await client.DeleteChannelAsync(groupId);
        }

        public static DiscordGroup LeaveGroup(this DiscordClient client, ulong groupId)
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

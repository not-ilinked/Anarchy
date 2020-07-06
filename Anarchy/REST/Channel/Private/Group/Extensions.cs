using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public static class GroupExtensions
    {
        /// <summary>
        /// Joins a group
        /// </summary>
        /// <param name="inviteCode">Invite for the group</param>
        /// <returns>The invite used</returns>
        public static DiscordInvite JoinGroup(this DiscordClient client, string inviteCode)
        {
            return client.HttpClient.Post($"/invites/{inviteCode}")
                                .Deserialize<DiscordInvite>().SetClient(client);
        }


        /// <summary>
        /// Creates a group
        /// </summary>
        /// <param name="recipients">The IDs of the recipients to add</param>
        /// <returns>The created <see cref="DiscordGroup"/></returns>
        public static DiscordGroup CreateGroup(this DiscordClient client, List<ulong> recipients)
        {
            return client.HttpClient.Post($"/users/@me/channels", new RecipientList() { Recipients = recipients })
                                .DeserializeEx<DiscordGroup>().SetClient(client);
        }


        // This does the same as DeleteChannel(), i just decided to leave it be because DeleteChannel() is a weird name for a function for leaving groups
        /// <summary>
        /// Leaves a group.
        /// </summary>
        /// <param name="groupId">ID of the group</param>
        /// <returns>The leaved <see cref="DiscordGroup"/></returns>
        public static DiscordChannel LeaveGroup(this DiscordClient client, ulong groupId)
        {
            return client.DeleteChannel(groupId);
        }


        /// <summary>
        /// Adds a user to a group
        /// </summary>
        /// <param name="groupId">ID of the group</param>
        /// <param name="userId">ID of the user</param>
        public static void AddUserToGroup(this DiscordClient client, ulong groupId, ulong userId)
        {
            client.HttpClient.Put($"/channels/{groupId}/recipients/{userId}");
        }


        /// <summary>
        /// Removes a user from a group
        /// </summary>
        /// <param name="groupId">ID of the group</param>
        /// <param name="userId">ID of the user</param>
        public static void RemoveUserFromGroup(this DiscordClient client, ulong groupId, ulong userId)
        {
            client.HttpClient.Delete($"/channels/{groupId}/recipients/{userId}");
        }
    }
}

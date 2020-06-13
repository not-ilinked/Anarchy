using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public static class RelationshipsExtensions
    {
        /// <summary>
        /// Gets the account's relationships (friends, blocked etc.)
        /// </summary>
        public static IReadOnlyList<Relationship> GetRelationships(this DiscordClient client)
        {
            return client.HttpClient.Get($"/users/@me/relationships")
                                .Deserialize<IReadOnlyList<Relationship>>().SetClientsInList(client);
        }


        public static void SendFriendRequest(this DiscordClient client, ulong userId)
        {
            client.HttpClient.Put("/users/@me/relationships/" + userId);
        }


        /// <summary>
        /// Sends a friend request to a user
        /// </summary>
        public static void SendFriendRequest(this DiscordClient client, string username, uint discriminator)
        {
            client.HttpClient.Post("/users/@me/relationships", $"{{\"username\":\"{username}\",\"discriminator\":{discriminator}}}");
        }


        /// <summary>
        /// Blocks a user
        /// </summary>
        /// <param name="userId">ID of the user</param>
        public static void BlockUser(this DiscordClient client, ulong userId)
        {
            client.HttpClient.Put($"/users/@me/relationships/{userId}", new Relationship() { Type = RelationshipType.Blocked });
        }


        /// <summary>
        /// Gets a user's profile
        /// </summary>
        /// <param name="userId">ID of the user</param>
        public static DiscordProfile GetProfile(this DiscordClient client, ulong userId)
        {
            return client.HttpClient.Get($"/users/{userId}/profile")
                                .Deserialize<DiscordProfile>().SetClient(client);
        }


        /// <summary>
        /// Gets the mutual friends between our user and the other user
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns></returns>
        public static IReadOnlyList<DiscordUser> GetMutualFriends(this DiscordClient client, long userId)
        {
            return client.HttpClient.Get($"/users/{userId}/relationships")
                                .Deserialize<IReadOnlyList<DiscordUser>>().SetClientsInList(client);
        }


        /// <summary>
        /// Removes any relationship (unfriending, unblocking etc.)
        /// </summary>
        /// <param name="userId">ID of the user</param>
        public static void RemoveRelationship(this DiscordClient client, ulong userId)
        {
            client.HttpClient.Delete($"/users/@me/relationships/{userId}");
        }
    }
}
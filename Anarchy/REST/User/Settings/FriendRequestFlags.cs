

using System.Text.Json.Serialization;

namespace Discord
{
    public class FriendRequestFlags
    {
        [JsonPropertyName("all")]
        public bool Everyone { get; private set; }

        [JsonPropertyName("mutual_friends")]
        public bool MutualFriends { get; private set; }

        [JsonPropertyName("mutual_guilds")]
        public bool MutualGuilds { get; private set; }
    }
}

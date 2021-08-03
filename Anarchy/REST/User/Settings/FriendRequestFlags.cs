using Newtonsoft.Json;

namespace Discord
{
    public class FriendRequestFlags
    {
        public FriendRequestFlags() { }

        public FriendRequestFlags(bool everyone, bool mutualFriends, bool mutualGuilds)
        {
            Everyone = everyone;
            MutualFriends = mutualFriends;
            MutualGuilds = mutualGuilds;
        }

        [JsonProperty("all")]
        public bool Everyone { get; private set; }

        [JsonProperty("mutual_friends")]
        public bool MutualFriends { get; private set; }

        [JsonProperty("mutual_guilds")]
        public bool MutualGuilds { get; private set; }
    }
}

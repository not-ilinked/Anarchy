using Newtonsoft.Json;

namespace Discord
{
    public class FriendRequestFlags
    {
        [JsonProperty("all")]
        public bool Everyone { get; set; }

        [JsonProperty("mutual_friends")]
        public bool MutualFriends { get; set; }

        [JsonProperty("mutual_guilds")]
        public bool MutualGuilds { get; set; }
    }
}

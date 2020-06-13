using Newtonsoft.Json;

namespace Discord
{
    public class FriendSourceFlags
    {
        private readonly Property<bool> AllProperty = new Property<bool>();
        [JsonProperty("all")]
        public bool All
        {
            get { return AllProperty; }
            set { AllProperty.Value = value; }
        }


        public bool ShouldSerializeAll()
        {
            return AllProperty.Set;
        }


        private readonly Property<bool> GuildsProperty = new Property<bool>();
        [JsonProperty("mutual_guilds")]
        public bool MutualGuilds
        {
            get { return GuildsProperty; }
            set { GuildsProperty.Value = value; }
        }


        public bool ShouldSerializeMutualGuilds()
        {
            return GuildsProperty.Set;
        }


        private readonly Property<bool> FriendsProperty = new Property<bool>();
        [JsonProperty("mutual_friends")]
        public bool MutualFriends
        {
            get { return FriendsProperty; }
            set { FriendsProperty.Value = value; }
        }


        public bool ShouldSerializeMutualFriends()
        {
            return FriendsProperty.Set;
        }
    }
}

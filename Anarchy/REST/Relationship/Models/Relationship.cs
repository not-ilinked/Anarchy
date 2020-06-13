using Newtonsoft.Json;

namespace Discord
{
    public class Relationship : Controllable
    {
        public Relationship()
        {
            OnClientUpdated += (sender, e) => User.SetClient(Client);
        }

        [JsonProperty("user")]
        public DiscordUser User { get; private set; }


        [JsonProperty("type")]
        public RelationshipType Type { get; internal set; }


        public void Remove()
        {
            Client.RemoveRelationship(User.Id);
        }


        public override string ToString()
        {
            return $"{Type} {User}";
        }


        public static implicit operator ulong(Relationship instance)
        {
            return instance.User.Id;
        }
    }
}
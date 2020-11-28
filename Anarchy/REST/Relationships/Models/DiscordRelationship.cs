using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Discord
{
    public class DiscordRelationship : Controllable
    {
        public DiscordRelationship()
        {
            OnClientUpdated += (sender, e) => User.SetClient(Client);
        }

        [JsonProperty("user")]
        public DiscordUser User { get; private set; }


        [JsonProperty("type")]
        public RelationshipType Type { get; internal set; }


        public async Task RemoveAsync()
        {
            await Client.RemoveRelationshipAsync(User.Id);
        }

        public void Remove()
        {
            RemoveAsync().GetAwaiter().GetResult();
        }


        public override string ToString()
        {
            return $"{Type} {User}";
        }


        public static implicit operator ulong(DiscordRelationship instance)
        {
            return instance.User.Id;
        }
    }
}
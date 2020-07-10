using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Discord
{
    public class DiscordGuildSubscription : Controllable
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }


        [JsonProperty("user_id")]
        public ulong UserId { get; private set; }


        [JsonProperty("guild_id")]
#pragma warning disable CS0649
        private readonly ulong _guildId;
#pragma warning restore CS0649

        public MinimalGuild Guild
        {
            get
            {
                return new MinimalGuild(_guildId).SetClient(Client);
            }
        }


        [JsonProperty("ended")]
        public bool Ended { get; private set; }


        public async Task RemoveAsync()
        {
            await Client.RemoveGuildBoostAsync(_guildId, Id);
        }

        public void Remove()
        {
            RemoveAsync().GetAwaiter().GetResult();
        }
    }
}

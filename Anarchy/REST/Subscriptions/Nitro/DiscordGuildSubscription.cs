using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Discord
{
    public class DiscordGuildSubscription : Controllable
    {
        [JsonPropertyName("id")]
        public ulong Id { get; private set; }

        [JsonPropertyName("user_id")]
        public ulong UserId { get; private set; }

        [JsonPropertyName("guild_id")]
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

        [JsonPropertyName("ended")]
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

using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class DeletedRole : Controllable
    {
        [JsonProperty("guild_id")]
        private readonly ulong _guildId;

        public MinimalGuild Guild
        {
            get { return new MinimalGuild(_guildId).SetClient(Client); }
        }

        [JsonProperty("role_id")]
        public ulong Id { get; private set; }
    }
}

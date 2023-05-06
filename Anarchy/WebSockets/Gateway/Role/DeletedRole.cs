

using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class DeletedRole : Controllable
    {
        [JsonPropertyName("guild_id")]
        private readonly ulong _guildId;

        public MinimalGuild Guild
        {
            get { return new MinimalGuild(_guildId).SetClient(Client); }
        }

        [JsonPropertyName("role_id")]
        public ulong Id { get; private set; }
    }
}

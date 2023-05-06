using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord
{
    public class GuildQueryResult : Controllable
    {
        public GuildQueryResult()
        {
            OnClientUpdated += (sender, e) =>
            {
                Guilds.SetClientsInList(Client);
            };
        }

        [JsonPropertyName("total")]
        public uint Total { get; private set; }

        [JsonPropertyName("guilds")]
        public IReadOnlyList<DiscoveryGuild> Guilds { get; private set; }
    }
}

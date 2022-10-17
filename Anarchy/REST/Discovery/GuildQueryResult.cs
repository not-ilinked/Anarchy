using System.Collections.Generic;
using Newtonsoft.Json;

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

        [JsonProperty("total")]
        public uint Total { get; private set; }

        [JsonProperty("guilds")]
        public IReadOnlyList<DiscoveryGuild> Guilds { get; private set; }
    }
}

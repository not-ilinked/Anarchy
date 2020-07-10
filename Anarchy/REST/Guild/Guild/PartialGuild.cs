using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord
{
    public class PartialGuild : BaseGuild
    {
        [JsonProperty("owner")]
        public bool Owner { get; private set; }


        [JsonProperty("permissions")]
        public DiscordPermission Permissions { get; private set; }


        [JsonProperty("features")]
        public IReadOnlyList<string> Features { get; private set; }


        public async Task<DiscordGuild> GetGuildAsync()
        {
            return await Client.GetGuildAsync(Id);
        }

        /// <summary>
        /// Gets the full guild (<see cref="DiscordGuild"/>)
        /// </summary>
        public DiscordGuild GetGuild()
        {
            return GetGuildAsync().Result;
        }
    }
}

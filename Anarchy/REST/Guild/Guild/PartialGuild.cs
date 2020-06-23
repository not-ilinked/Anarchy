using Newtonsoft.Json;
using System.Collections.Generic;

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


        /// <summary>
        /// Gets the full guild (<see cref="DiscordGuild"/>)
        /// </summary>
        public DiscordGuild GetGuild()
        {
            return Client.GetGuild(Id);
        }
    }
}

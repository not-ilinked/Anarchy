using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Discord
{
    public class PartialGuild : BaseGuild
    {
        [JsonPropertyName("owner")]
        public bool Owner { get; private set; }

        [JsonPropertyName("permissions")]
        public DiscordPermission Permissions { get; private set; }

        [JsonPropertyName("features")]
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
            return GetGuildAsync().GetAwaiter().GetResult();
        }
    }
}

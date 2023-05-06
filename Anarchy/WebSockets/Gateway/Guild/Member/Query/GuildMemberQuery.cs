

using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    /// <summary>
    /// Query for getting a list of guild members
    /// </summary>
    public class GuildMemberQuery
    {
        [JsonPropertyName("guild_id")]
        public ulong GuildId { get; set; }

        [JsonPropertyName("query")]
        private readonly string _query = "";

        [JsonPropertyName("limit")]
        public uint Limit { get; set; }

        public override string ToString()
        {
            return GuildId.ToString();
        }
    }
}

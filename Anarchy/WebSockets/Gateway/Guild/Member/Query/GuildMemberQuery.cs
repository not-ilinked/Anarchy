using Newtonsoft.Json;

namespace Discord.Gateway
{
    /// <summary>
    /// Query for getting a list of guild members
    /// </summary>
    public class GuildMemberQuery
    {
        [JsonProperty("guild_id")]
        public ulong GuildId { get; set; }


        [JsonProperty("query")]
        private readonly string _query = "";


        [JsonProperty("limit")]
        public uint Limit { get; set; }

        public override string ToString()
        {
            return GuildId.ToString();
        }
    }
}

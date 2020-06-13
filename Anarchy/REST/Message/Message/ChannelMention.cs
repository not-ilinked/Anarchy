using Newtonsoft.Json;

namespace Discord
{
    public class ChannelMention
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }


        [JsonProperty("guild_id")]
        public ulong GuildId { get; private set; }


        [JsonProperty("type")]
        public ChannelType Type { get; private set; }


        [JsonProperty("name")]
        public string Name { get; private set; }
    }
}

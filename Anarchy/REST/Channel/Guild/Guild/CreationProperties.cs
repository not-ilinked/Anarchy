using Newtonsoft.Json;

namespace Discord
{
    internal class GuildChannelCreationProperties : ChannelCreationProperties
    {
        [JsonProperty("parent_id")]
        public ulong? ParentId { get; set; }
    }
}
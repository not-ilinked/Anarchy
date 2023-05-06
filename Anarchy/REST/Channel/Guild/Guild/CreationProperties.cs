using System.Text.Json.Serialization;

namespace Discord
{
    internal class GuildChannelCreationProperties : ChannelCreationProperties
    {
        [JsonPropertyName("parent_id")]
        public ulong? ParentId { get; set; }
    }
}
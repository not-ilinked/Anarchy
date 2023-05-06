

using System.Text.Json.Serialization;

namespace Discord
{
    public class DiscordSticker
    {
        [JsonPropertyName("id")]
        public ulong Id { get; private set; }

        [JsonPropertyName("pack_id")]
        public ulong PackId { get; private set; }

        [JsonPropertyName("name")]
        public string Name { get; private set; }

        [JsonPropertyName("description")]
        public string Description { get; private set; }

        [JsonPropertyName("asset")]
        public string AssetHash { get; private set; }

        [JsonPropertyName("format_type")]
        public StickerFormatType FormatType { get; private set; }
    }
}

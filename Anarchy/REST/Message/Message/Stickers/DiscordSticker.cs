using Newtonsoft.Json;

namespace Discord
{
    public class DiscordSticker
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }

        [JsonProperty("pack_id")]
        public ulong PackId { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("asset")]
        public string AssetHash { get; private set; }

        [JsonProperty("format_type")]
        public StickerFormatType FormatType { get; private set; }
    }
}

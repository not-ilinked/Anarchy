

using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class GiftCodeCreatedEventArgs : Controllable
    {
        [JsonPropertyName("sku_id")]
        public ulong SkuId { get; private set; }

        [JsonPropertyName("code")]
        public string Code { get; private set; }
    }
}

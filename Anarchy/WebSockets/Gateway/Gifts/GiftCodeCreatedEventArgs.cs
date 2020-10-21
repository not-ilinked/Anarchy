using System;
using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class GiftCodeCreatedEventArgs : Controllable
    {
        [JsonProperty("sku_id")]
        public ulong SkuId { get; private set; }


        [JsonProperty("code")]
        public string Code { get; private set; }
    }
}

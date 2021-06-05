using Newtonsoft.Json;

namespace Discord
{
    internal class InteractionResponse
    {
        [JsonProperty("type")]
        public InteractionCallbackType Type { get; set; }

        [JsonProperty("data")]
        public InteractionResponseProperties Data { get; set; }
    }
}

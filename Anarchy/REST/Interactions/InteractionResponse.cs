

using System.Text.Json.Serialization;

namespace Discord
{
    internal class InteractionResponse
    {
        [JsonPropertyName("type")]
        public InteractionCallbackType Type { get; set; }

        [JsonPropertyName("data")]
        public InteractionResponseProperties Data { get; set; }
    }
}

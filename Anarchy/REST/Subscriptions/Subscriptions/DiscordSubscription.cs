

using System.Text.Json.Serialization;

namespace Discord
{
    public class DiscordSubscription
    {
        [JsonPropertyName("id")]
        public ulong Id { get; private set; }

        [JsonPropertyName("plan_id")]
        public ulong PlanId { get; private set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; private set; }
    }
}

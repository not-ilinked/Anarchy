

using System.Text.Json.Serialization;

namespace Discord
{
    public class AdditionalSubscriptionPlan
    {
        [JsonPropertyName("plan_id")]
        public ulong Id { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; } = 1;
    }
}

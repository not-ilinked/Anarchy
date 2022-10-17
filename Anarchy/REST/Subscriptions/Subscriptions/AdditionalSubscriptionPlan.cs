using Newtonsoft.Json;

namespace Discord
{
    public class AdditionalSubscriptionPlan
    {
        [JsonProperty("plan_id")]
        public ulong Id { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; } = 1;
    }
}

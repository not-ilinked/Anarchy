using Newtonsoft.Json;

namespace Discord
{
    public class DiscordSubscription
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }

        [JsonProperty("plan_id")]
        public ulong PlanId { get; private set; }

        [JsonProperty("quantity")]
        public int Quantity { get; private set; }
    }
}

using Newtonsoft.Json;

namespace Discord
{
    public class CardPaymentMethod : PaymentMethod
    {
        [JsonProperty("brand")]
        public string Brand { get; private set; }


        [JsonProperty("country")]
        public string Country { get; private set; }


        [JsonProperty("last_4")]
        public int Last4 { get; private set; }


        [JsonProperty("expires_month")]
        public int ExpirationMonth { get; private set; }


        [JsonProperty("expires_year")]
        public int ExpirationYear { get; private set; }
    }
}

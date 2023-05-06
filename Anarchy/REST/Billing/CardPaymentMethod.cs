using System.Text.Json.Serialization;

namespace Discord
{
    public class CardPaymentMethod : PaymentMethod
    {
        [JsonPropertyName("brand")]
        public string Brand { get; private set; }

        [JsonPropertyName("country")]
        public string Country { get; private set; }

        [JsonPropertyName("last_4")]
        public int Last4 { get; private set; }

        [JsonPropertyName("expires_month")]
        public int ExpirationMonth { get; private set; }

        [JsonPropertyName("expires_year")]
        public int ExpirationYear { get; private set; }
    }
}
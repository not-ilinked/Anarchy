using Newtonsoft.Json;

namespace Discord
{
    public class PayPalPaymentMethod : PaymentMethod
    {
        [JsonProperty("email")]
        public string Email { get; private set; }
    }
}

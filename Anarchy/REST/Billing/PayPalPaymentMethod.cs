using System.Text.Json.Serialization;

namespace Discord
{
    public class PayPalPaymentMethod : PaymentMethod
    {
        [JsonPropertyName("email")]
        public string Email { get; private set; }
    }
}
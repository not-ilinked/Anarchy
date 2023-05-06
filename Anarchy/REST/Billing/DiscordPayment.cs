using System;
using System.Text.Json.Serialization;

namespace Discord
{
    public class DiscordPayment : Controllable
    {
        public DiscordPayment()
        {
            OnClientUpdated += (sender, e) => PaymentMethod.SetClient(Client);
        }

        [JsonPropertyName("id")]
        public ulong Id { get; private set; }

        [JsonPropertyName("created_at")]
        public DateTime Timestamp { get; private set; }

        [JsonPropertyName("currency")]
        public string Currency { get; private set; }

        [JsonPropertyName("amount")]
        public string Amount { get; private set; }

        [JsonPropertyName("status")]
        public DiscordPaymentStatus Status { get; private set; }

        [JsonPropertyName("description")]
        public string Description { get; private set; }

        [JsonPropertyName("flags")]
        public DiscordPaymentFlags Flags { get; private set; }

        [JsonPropertyName("payment_source")]
        [JsonConverter(typeof(DeepJsonConverter<PaymentMethod>))]
        public PaymentMethod PaymentMethod { get; private set; }
    }
}
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Discord
{
    public class BillingAddress
    {
        [JsonPropertyName("name")]
        public string Name { get; private set; }

        [JsonPropertyName("line_1")]
        public string Address1 { get; private set; }

        [JsonPropertyName("line_2")]
        public string Address2 { get; private set; }

        [JsonPropertyName("city")]
        public string City { get; private set; }

        [JsonPropertyName("state")]
        public string State { get; private set; }

        [JsonPropertyName("country")]
        public string Country { get; private set; }

        [JsonPropertyName("postal_code")]
        public JsonElement PostalCode { get; private set; }
    }
}
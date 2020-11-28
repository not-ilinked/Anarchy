using Newtonsoft.Json;

namespace Discord
{
    public class BillingAddress
    {
        [JsonProperty("name")]
        public string Name { get; private set; }


        [JsonProperty("line_1")]
        public string Address1 { get; private set; }


        [JsonProperty("line_2")]
        public string Address2 { get; private set; }


        [JsonProperty("city")]
        public string City { get; private set; }


        [JsonProperty("state")]
        public string State { get; private set; }


        [JsonProperty("country")]
        public string Country { get; private set; }


        [JsonProperty("postal_code")]
        public dynamic PostalCode { get; private set; }
    }
}

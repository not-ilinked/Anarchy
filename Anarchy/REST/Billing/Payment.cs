using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Discord
{
    public class Payment
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }


        [JsonProperty("created_at")]
        public string Timestamp { get; private set; }


        [JsonProperty("currency")]
        public string Currency { get; private set; }


        [JsonProperty("amount")]
        public string Amount { get; private set; }


        [JsonProperty("status")]
        public int Status { get; private set; } // not sure what htis is lol


        [JsonProperty("description")]
        public string Description { get; private set; }


        [JsonProperty("flags")]
        public int Flags { get; private set; }


        [JsonProperty("payment_source")]
        public PaymentMethod PaymentMethod { get; private set; }
    }
}

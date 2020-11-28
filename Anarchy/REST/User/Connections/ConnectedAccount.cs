using Newtonsoft.Json;
using System;

namespace Discord
{
    public class ConnectedAccount : Controllable
    {
        [JsonProperty("id")]
        public string Id { get; private set; }


        [JsonProperty("type")]
        private readonly string _type;
        public ConnectedAccountType Type
        {
            get
            {
                return (ConnectedAccountType)Enum.Parse(typeof(ConnectedAccountType), _type, true);
            }
        }


        [JsonProperty("name")]
        public string Name { get; protected set; }


        [JsonProperty("verified")]
        public bool Verified { get; protected set; }


        public override string ToString()
        {
            return Name;
        }
    }
}

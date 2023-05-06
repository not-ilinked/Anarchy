using System;
using System.Text.Json.Serialization;

namespace Discord
{
    public class ConnectedAccount : Controllable
    {
        [JsonPropertyName("id")]
        public string Id { get; private set; }

        [JsonPropertyName("type")]
        private readonly string _type;
        public ConnectedAccountType Type
        {
            get
            {
                return (ConnectedAccountType) Enum.Parse(typeof(ConnectedAccountType), _type, true);
            }
        }

        [JsonPropertyName("name")]
        public string Name { get; protected set; }

        [JsonPropertyName("verified")]
        public bool Verified { get; protected set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

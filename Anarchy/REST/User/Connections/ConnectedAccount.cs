using Newtonsoft.Json;
using System;

namespace Discord
{
    public class ConnectedAccount : Controllable
    {
        [JsonProperty("id")]
        public string Id { get; private set; }


        [JsonProperty("type")]
#pragma warning disable CS0649
        private readonly string _type;
#pragma warning restore CS0649
        public AccountType Type
        {
            get
            {
                return (AccountType)Enum.Parse(typeof(AccountType), _type, true);
            }
        }


        [JsonProperty("name")]
        public string Name { get; private set; }


        [JsonProperty("verified")]
        public bool Verified { get; private set; }


        public override string ToString()
        {
            return Name;
        }


        public static implicit operator string(ConnectedAccount instance)
        {
            return instance.Id;
        }
    }
}

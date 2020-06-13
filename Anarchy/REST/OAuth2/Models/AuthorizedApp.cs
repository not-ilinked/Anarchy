using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord
{
    public class AuthorizedApp : Controllable
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }


        [JsonProperty("scopes")]
        public IReadOnlyList<string> Scopes { get; private set; }


        [JsonProperty("application")]
        public OAuth2Application Application { get; private set; }


        public void Deauthorize()
        {
            Client.DeauthorizeApp(Id);
        }


        public static implicit operator ulong(AuthorizedApp instance)
        {
            return instance.Id;
        }
    }
}

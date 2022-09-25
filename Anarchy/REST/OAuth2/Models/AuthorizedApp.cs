using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task DeauthorizeAsync()
        {
            await Client.DeauthorizeAppAsync(Id);
        }

        public void Deauthorize()
        {
            DeauthorizeAsync().GetAwaiter().GetResult();
        }

        public static implicit operator ulong(AuthorizedApp instance)
        {
            return instance.Id;
        }
    }
}

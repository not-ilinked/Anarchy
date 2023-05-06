using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace Discord
{
    public class AuthorizedApp : Controllable
    {
        [JsonPropertyName("id")]
        public ulong Id { get; private set; }

        [JsonPropertyName("scopes")]
        public IReadOnlyList<string> Scopes { get; private set; }

        [JsonPropertyName("application")]
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

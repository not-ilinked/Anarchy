using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Discord
{
    public class ClientConnectedAccount : ConnectedAccount
    {
        [JsonProperty("visibility")]
        public bool Visible { get; private set; }


        [JsonProperty("show_activity")]
        public bool ShowActivity { get; private set; }


        public async Task RemoveAsync()
        {
            await Client.RemoveConnectedAccountAsync(Type, Id);
        }

        public void Remove()
        {
            RemoveAsync().GetAwaiter().GetResult();
        }
    }
}

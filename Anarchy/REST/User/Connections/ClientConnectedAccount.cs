using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Discord
{
    public class ClientConnectedAccount : ConnectedAccount
    {
        [JsonProperty("visibility")]
        public bool Visible { get; private set; }

        [JsonProperty("show_activity")]
        public bool ShowAsActivity { get; private set; }

        public async Task ModifyAsync(ConnectionProperties properties)
        {
            var update = await Client.ModifyConnectedAccountAsync(Type, Id, properties);

            Name = update.Name;
            Verified = update.Verified;
            Visible = update.Visible;
            ShowAsActivity = update.ShowAsActivity;
        }

        public void Modify(ConnectionProperties properties)
        {
            ModifyAsync(properties).GetAwaiter().GetResult();
        }

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

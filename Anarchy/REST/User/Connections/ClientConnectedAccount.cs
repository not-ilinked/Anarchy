using Newtonsoft.Json;

namespace Discord
{
    public class ClientConnectedAccount : ConnectedAccount
    {
        [JsonProperty("visibility")]
        public bool Visible { get; private set; }


        [JsonProperty("show_activity")]
        public bool ShowActivity { get; private set; }

        public void Remove()
        {
            Client.RemoveConnectedAccount(Type, Id);
        }
    }
}

using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class ChannelRecipientEventArgs : Controllable
    {
        public ChannelRecipientEventArgs()
        {
            OnClientUpdated += (sender, e) => User.SetClient(Client);
        }

        [JsonProperty("user")]
        public DiscordUser User { get; private set; }


        [JsonProperty("channel_id")]
        private readonly ulong _channelId;

        public MinimalChannel Channel
        {
            get { return new MinimalChannel(_channelId).SetClient(Client); }
        }
    }
}

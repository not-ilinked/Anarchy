using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class ChannelRecipientUpdate : Controllable
    {
        public ChannelRecipientUpdate()
        {
            OnClientUpdated += (sender, e) =>
            {
                User.SetClient(Client);
            };
        }


        [JsonProperty("user")]
        public DiscordUser User { get; private set; }


        [JsonProperty("channel_id")]
        private ulong _channelId;

        public MinimalChannel Channel
        {
            get { return new MinimalChannel(_channelId); }
        }
    }
}

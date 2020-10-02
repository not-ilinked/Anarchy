using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class ChannelUnreadMessages : Controllable
    {
        [JsonProperty("id")]
        private readonly ulong _channelId;

        public MinimalTextChannel Channel
        {
            get { return new MinimalTextChannel(_channelId).SetClient(Client); }
        }

        [JsonProperty("last_message_id")]
        public ulong LastMessageId { get; private set; }
    }
}

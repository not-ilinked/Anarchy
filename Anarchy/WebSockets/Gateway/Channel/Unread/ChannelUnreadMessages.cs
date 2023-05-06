

using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class ChannelUnreadMessages : Controllable
    {
        [JsonPropertyName("id")]
        private readonly ulong _channelId;

        public MinimalTextChannel Channel
        {
            get { return new MinimalTextChannel(_channelId).SetClient(Client); }
        }

        [JsonPropertyName("last_message_id")]
        public ulong LastMessageId { get; private set; }
    }
}

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class DiscordCall : Controllable
    {
        [JsonProperty("channel_id")]
        private readonly ulong _channelId;

        public MinimalTextChannel Channel
        {
            get { return new MinimalTextChannel(_channelId).SetClient(Client); }
        }


        [JsonProperty("message_id")]
        public ulong MessageId { get; private set; }


        [JsonProperty("region")]
        public string Region { get; private set; }


        [JsonProperty("ringing")]
        public IReadOnlyList<ulong> Ringing { get; private set; }
    }
}

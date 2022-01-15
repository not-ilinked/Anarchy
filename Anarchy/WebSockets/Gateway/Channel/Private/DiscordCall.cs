using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord.Gateway
{
    public class DiscordCall : Controllable
    {
        [JsonProperty("channel_id")]
        private readonly ulong _channelId;

        public MinimalTextChannel Channel => new MinimalTextChannel(_channelId).SetClient(Client);


        [JsonProperty("message_id")]
        public ulong MessageId { get; private set; }


        [JsonProperty("region")]
        public string Region { get; private set; }


        [JsonProperty("ringing")]
        public IReadOnlyList<ulong> Ringing { get; private set; }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Discord.WebSockets
{
    public class DiscordWebSocketMessage<TOpcode> : DiscordWebSocketRequest<JToken, TOpcode> where TOpcode : Enum
    {
        // these members only apply to the Gateway :P
        [JsonProperty("t")]
        public string EventName { get; private set; }


        [JsonProperty("s")]
        public uint? Sequence { get; private set; }
    }
}

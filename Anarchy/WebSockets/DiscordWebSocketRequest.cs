using System;
using Newtonsoft.Json;

namespace Discord.WebSockets
{
    public class DiscordWebSocketRequest<TData, TOpcode> where TOpcode : Enum
    {
        public DiscordWebSocketRequest()
        { }

        public DiscordWebSocketRequest(TOpcode op, TData data)
        {
            Opcode = op;
            Data = data;
        }

        [JsonProperty("op")]
        public TOpcode Opcode { get; private set; }

        [JsonProperty("d")]
        public TData Data { get; private set; }
    }
}

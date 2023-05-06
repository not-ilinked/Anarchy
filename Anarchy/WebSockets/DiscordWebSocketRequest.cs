using System;
using System.Text.Json.Serialization;

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

        [JsonPropertyName("op")]
        public TOpcode Opcode { get; private set; }

        [JsonPropertyName("d")]
        public TData Data { get; private set; }
    }
}

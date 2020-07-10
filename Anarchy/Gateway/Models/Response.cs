using Newtonsoft.Json;

namespace Discord.Gateway
{
    /// <summary>
    /// A response from the gateway
    /// </summary>
    internal class GatewayResponse
    {
        [JsonProperty("op")]
        public GatewayOpcode Opcode { get; private set; }


        [JsonProperty("t")]
        public string Title { get; private set; }


        [JsonProperty("d")]
        public object Data { get; private set; }


        [JsonProperty("s")]
        public uint? Sequence { get; private set; }


        public override string ToString()
        {
            return Opcode.ToString() + (Title != null ? $" {Title}" : "");
        }
    }
}
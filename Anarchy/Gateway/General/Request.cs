using Newtonsoft.Json;

namespace Discord.Gateway
{
    /// <summary>
    /// A request sent to the gateway
    /// </summary>
    /// <typeparam name="T">Type of data</typeparam>
    internal class GatewayRequest<T> where T : new()
    {
        public GatewayRequest(GatewayOpcode opcode)
        {
            Data = new T();
            Opcode = opcode;
        }


        [JsonProperty("op")]
        public GatewayOpcode Opcode { get; private set; }


        [JsonProperty("d")]
        public T Data { get; set; }


        public override string ToString()
        {
            return Opcode.ToString();
        }
    }
}
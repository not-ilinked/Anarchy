using Newtonsoft.Json;

namespace Discord.Gateway
{
    internal static class GatewayExtensions
    {
        internal static void Send<T>(this DiscordSocketClient client, GatewayOpcode op, T requestData) where T : new()
        {
            client.Socket.Send(JsonConvert.SerializeObject(new GatewayRequest<T>(op) { Data = requestData }));
        }
    }
}
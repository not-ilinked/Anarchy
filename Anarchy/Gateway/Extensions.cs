using Newtonsoft.Json;
using System;
using System.Threading;

namespace Discord.Gateway
{
    public static class GatewayExtensions
    {
        public static void Send<T>(this DiscordSocketClient client, GatewayOpcode op, T requestData) where T : new()
        {
            lock (client.RequestLock)
            {
                if (client.Cooldown > DateTime.Now)
                    Thread.Sleep(client.Cooldown - DateTime.Now);

                client.Socket.Send(JsonConvert.SerializeObject(new GatewayRequest<T>(op) { Data = requestData }));

                client.Cooldown = DateTime.Now + new TimeSpan(0, 0, 0, 0, 500);
            }
        }
    }
}
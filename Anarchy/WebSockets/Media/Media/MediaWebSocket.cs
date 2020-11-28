using Discord.WebSockets;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Discord.Media
{
    internal class MediaWebSocket : DiscordWebSocket<DiscordMediaOpcode>
    {
        private static int _next;
        public int Id { get; }

        private readonly DiscordMediaSession _parent;

        public MediaWebSocket(string url, DiscordMediaSession parent) : base(url)
        {
            Id = _next++;
            _parent = parent;
        }


        public void SelectProtocol(IPEndPoint localEndpoint)
        {
            _parent.Log("Selecting protocol");

            Send(DiscordMediaOpcode.SelectProtocol, new MediaProtocolSelection()
            {
                Protocol = "udp",
                ProtocolData = new MediaProtocolData()
                {
                    Host = localEndpoint.Address.ToString(),
                    Port = localEndpoint.Port,
                    EncryptionMode = Sodium.EncryptionMode
                },
                RtcConnectionId = Guid.NewGuid().ToString(),
                Codecs = DiscordMediaSession.SupportedCodecs.Values.ToList()
            });
        }

        public async void StartHeartbeaterAsync(int interval)
        {
            try
            {
                while (true)
                {
                    Send(DiscordMediaOpcode.Heartbeat, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                    await Task.Delay(interval);
                }
            }
            catch (InvalidOperationException) { }
        }
    }
}

using Discord.Gateway;
using Discord.Voice;
using Leaf.xNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Discord.Media
{
    public abstract class DiscordMediaSession
    {
        private readonly Random _rng;

        private readonly WebSocket _socket;
        public DiscordSocketClient Client { get; private set; }

        private IPEndPoint _serverEndpoint;
        private IPEndPoint _localEndpoint;

        internal UdpClient UdpClient { get; set; }
        internal SSRCUpdate SSRC { get; private set; }
        internal byte[] SecretKey { get; private set; }

        public DiscordMediaServer Server { get; private set; }

        private readonly ulong _channelId;
        public MinimalChannel Channel
        {
            get
            {
                return new MinimalTextChannel(_channelId).SetClient(Client);
            }
        }

        public DiscordMediaClientState State { get; private set; }

        internal DiscordMediaSession(DiscordSocketClient client, DiscordMediaServer server, ulong channelId)
        {
            _rng = new Random();

            Client = client;
            Server = server;
            _channelId = channelId;

            _socket = new WebSocket("wss://" + Server.Server.Split(':')[0] + "?v=4");
            UdpClient = new UdpClient();

            if (Client.Config.Proxy != null)
            {
                if (Client.Config.Proxy.Type == ProxyType.HTTP) //WebSocketSharp only supports HTTP proxies :(
                    _socket.SetProxy("http://" + Client.Config.Proxy, "", "");
            }

            _socket.OnClose += (sender, e) => HandleDisconnect(new DiscordMediaCloseEventArgs((DiscordMediaCloseError)e.Code));

            _socket.OnMessage += Socket_OnMessage;
            _socket.Connect();

            State = DiscordMediaClientState.Connecting;
        }


        protected void Send<T>(DiscordMediaOpcode op, T payload)
        {
            _socket.Send(JsonConvert.SerializeObject(new DiscordMediaRequest<T>()
            {
                Opcode = op,
                Payload = payload
            }));
        }


        /// <summary>
        /// Disconnects from the current voice channel
        /// </summary>
        public virtual void Disconnect()
        {
            _socket.Close();

            UdpClient.Close();
        }


        protected uint GenerateSSRC()
        {
            return (uint)_rng.Next(0, 1000000);
        }


        protected abstract ulong GetServerId();

        protected virtual void HandleResponse(DiscordMediaResponse response) { }
        protected virtual void HandlePacket(RTPPacketHeader header, byte[] payload) { }

        protected abstract void HandleConnect();
        protected abstract void HandleDisconnect(DiscordMediaCloseEventArgs args);


        private void Socket_OnMessage(object sender, WebSocketSharp.MessageEventArgs e)
        {
            var payload = e.Data.Deserialize<DiscordMediaResponse>();

            Task.Run(() =>
            {
                switch (payload.Opcode)
                {
                    case DiscordMediaOpcode.Ready:
                        DiscordMediaReady ready = payload.Deserialize<DiscordMediaReady>();

                        SSRC = new SSRCUpdate() { Audio = (uint)ready.SSRC };

                        _serverEndpoint = new IPEndPoint(IPAddress.Parse(ready.IP), ready.Port);

                        UdpClient.Connect(_serverEndpoint);

                        // Hole punch receiver
                        Task.Run(() =>
                        {
                            var bytes = UdpClient.Receive(ref _serverEndpoint);

                            string ip = "";

                            for (int i = 8; i < bytes.Length; i++)
                            {
                                if (bytes[i] == 0)
                                    break;
                                else
                                    ip += (char)bytes[i];
                            }

                            _localEndpoint = new IPEndPoint(IPAddress.Parse(ip), BitConverter.ToUInt16(new byte[] { bytes[bytes.Length - 1], bytes[bytes.Length - 2] }, 0));

                            SelectProtocol();
                        });

                        Holepunch();

                        break;
                    case DiscordMediaOpcode.SessionDescription:
                        List<byte> why = new List<byte>();

                        foreach (byte item in payload.Deserialize<dynamic>().secret_key)
                            why.Add(item);

                        SecretKey = why.ToArray();

                        SSRC = new SSRCUpdate() { Audio = SSRC.Audio, Video = SSRC.Audio + 1, Rtx = SSRC.Audio + 2 };

                        Send(DiscordMediaOpcode.SSRCUpdate, SSRC);

                        State = DiscordMediaClientState.Connected;

                        Task.Run(() => StartListener());

                        HandleConnect();
                        break;
                    case DiscordMediaOpcode.Hello:
                        Send(DiscordMediaOpcode.Identify, new DiscordMediaIdentify()
                        {
                            ServerId = GetServerId(),
                            UserId = Client.User.Id,
                            SessionId = Client.SessionId,
                            Token = Server.Token,
                            Video = true
                        });

                        try
                        {
                            while (true)
                            {
                                Send(DiscordMediaOpcode.Heartbeat, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                                Thread.Sleep((int)payload.Deserialize<dynamic>().heartbeat_interval);
                            }
                        }
                        catch { }

                        break;
                    default:
                        HandleResponse(payload);
                        break;
                }
            });
        }


        private void Holepunch()
        {
            byte[] payload = new byte[74];
            payload[0] = 1 >> 8;
            payload[1] = 1 >> 0;
            payload[2] = 0x46 >> 8;
            payload[3] = 0x46 >> 0;
            payload[4] = (byte)(SSRC.Audio >> 24);
            payload[5] = (byte)(SSRC.Audio >> 16);
            payload[6] = (byte)(SSRC.Audio >> 8);
            payload[7] = (byte)(SSRC.Audio >> 0);

            UdpClient.Send(payload, payload.Length);
        }


        private void SelectProtocol()
        {
            Send(DiscordMediaOpcode.SelectProtocol, new MediaProtocolSelection()
            {
                Protocol = "udp",
                ProtocolData = new MediaProtocolData()
                {
                    Host = _localEndpoint.Address.ToString(),
                    Port = _localEndpoint.Port,
                    EncryptionMode = Sodium.EncryptionMode
                },
                RtcConnectionId = Guid.NewGuid().ToString(),
                Codecs = new List<MediaCodec>() { OpusEncoder.Codec, H264Packager.Codec }
            });
        }


        private void StartListener()
        {
            while (true)
            {
                byte[] received = UdpClient.Receive(ref _localEndpoint);

                if (received[0] == 0x80 || received[0] == 0x90)
                {
                    var header = RTPPacketHeader.Read(SecretKey, received, out byte[] payload);

                    HandlePacket(header, payload);
                }
            }
        }
    }
}

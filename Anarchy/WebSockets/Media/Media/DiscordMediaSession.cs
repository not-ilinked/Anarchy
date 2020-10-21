using Discord.Gateway;
using Discord.WebSockets;
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
    public abstract class DiscordMediaSession : IDisposable
    {
        public delegate void UpdateHandler(DiscordMediaSession session, DiscordMediaServer server);
        public event UpdateHandler OnServerUpdated;

        internal DiscordWebSocket<DiscordMediaOpcode> WebSocket { get; private set; }
        public DiscordSocketClient Client { get; private set; }

        private IPEndPoint _serverEndpoint;
        private IPEndPoint _localEndpoint;

        private List<KeyValuePair<DiscordMediaOpcode, object>> _heldBackMessages;

        internal UdpClient UdpClient { get; set; }
        internal DiscordSSRC SSRC { get; set; }
        internal byte[] SecretKey { get; private set; }
        public string SessionId { get; private set; }
        internal static readonly Dictionary<string, MediaCodec> SupportedCodecs = new Dictionary<string, MediaCodec>()
        {
            { "opus", new MediaCodec() { Name = "opus", Type = CodecType.Audio, PayloadType = 120, Priority = 1000 } },
            { "H264", new VideoMediaCodec() { Name = "H264", Type = CodecType.Video, PayloadType = 101, Priority = 1000, RtxPayloadType = 102 } }
        };

        public DiscordMediaServer Server { get; internal set; }
        public bool ReceivePackets { get; set; } = true;

        internal ulong? GuildId { get; private set; }
        public MinimalGuild Guild
        {
            get
            {
                if (GuildId.HasValue)
                    return new MinimalGuild(GuildId.Value).SetClient(Client);
                else
                    return null;
            }
        }

        internal ulong ChannelId { get; set; }
        public MinimalChannel Channel
        {
            get { return new MinimalTextChannel(ChannelId).SetClient(Client); }
        }

        public MediaSessionState State { get; protected set; }

        internal DiscordMediaSession(DiscordSocketClient client, ulong? guildId, ulong channelId, string sessionId)
        {
            Client = client;
            GuildId = guildId;
            ChannelId = channelId;
            SessionId = sessionId;
            _heldBackMessages = new List<KeyValuePair<DiscordMediaOpcode, object>>();
        }

        // If u connect to a vc in the same server as ur previous, ur connection won't die
        // It makes no sense to then manually kill it bcuz that's extra time
        // There's a chance that the client will miss events. A low one, but quite possible
        internal DiscordMediaSession(DiscordMediaSession other) : this(other.Client, other.GuildId, other.ChannelId, other.SessionId)
        {
            other.State = MediaSessionState.NotConnected;
            other.OnServerUpdated = null;
            other.WebSocket.OnMessageReceived -= OnMessageReceived;
            other.WebSocket.OnClosed -= OnClosed;

            WebSocket = other.WebSocket;
            _serverEndpoint = other._serverEndpoint;
            _localEndpoint = other._localEndpoint;
            UdpClient = other.UdpClient;
            SSRC = other.SSRC;
            SecretKey = other.SecretKey;
            SessionId = other.SessionId;
            Server = other.Server;
            ReceivePackets = other.ReceivePackets;
            State = MediaSessionState.Connected;
        }


        public void Connect()
        {
            if (State == MediaSessionState.Connected)
                Task.Run(() => HandleConnect());
            else
            {
                WebSocket = new DiscordWebSocket<DiscordMediaOpcode>("wss://" + Server.Endpoint + "?v=4");
                WebSocket.OnClosed += OnClosed;
                WebSocket.OnMessageReceived += OnMessageReceived;

                State = MediaSessionState.Connecting;

                WebSocket.SetProxy(Client.Proxy);
                WebSocket.Connect();
            }
        }


        internal void SafeSend<T>(DiscordMediaOpcode op, T data)
        {
            if (State == MediaSessionState.Connecting)
                _heldBackMessages.Add(new KeyValuePair<DiscordMediaOpcode, object>(op, data));
            else if (State == MediaSessionState.Connected)
                WebSocket.Send(op, data);
            else
                throw new InvalidOperationException("Not connected");
        }


        internal void UpdateServer(DiscordMediaServer server)
        {
            Server = server;

            if (server.Endpoint == null)
                State = MediaSessionState.Connecting;
            else if (WebSocket != null)
                Disconnect((ushort)UnorthodoxCloseCode.SwitchingServer, "Changing server");

            if (OnServerUpdated != null)
                Task.Run(() => OnServerUpdated.Invoke(this, server));
        }


        private void OnClosed(object sender, CloseEventArgs args)
        {
            if (State == MediaSessionState.Connected)
                UdpClient.Close();

            bool lostConnection = args.Code == 1006;

            if (lostConnection)
                Thread.Sleep(200);

            if (lostConnection || args.Code == (ushort)UnorthodoxCloseCode.SwitchingServer)
                Task.Run(() => Connect());
            else if (args.Code != (ushort)UnorthodoxCloseCode.KeepQuiet)
                Task.Run(() => HandleDisconnect(new DiscordMediaCloseEventArgs((DiscordMediaCloseCode)args.Code, args.Reason)));
        }


        private void OnMessageReceived(object sender, DiscordWebSocketMessage<DiscordMediaOpcode> message)
        {
            try
            {
                Console.WriteLine(message.Opcode);

                switch (message.Opcode)
                {
                    case DiscordMediaOpcode.Ready:
                        DiscordMediaReady ready = message.Data.ToObject<DiscordMediaReady>();

                        SSRC = new DiscordSSRC() { Audio = ready.SSRC };

                        _serverEndpoint = new IPEndPoint(IPAddress.Parse(ready.IP), ready.Port);

                        UdpClient = new UdpClient();
                        UdpClient.Connect(_serverEndpoint);

                        Task.Run(() => StartListener());

                        Holepunch();

                        break;
                    case DiscordMediaOpcode.SessionDescription:
                        var description = message.Data.ToObject<DiscordSessionDescription>();

                        if (ValidateCodecs(description))
                        {
                            if (description.EncryptionMode != Sodium.EncryptionMode)
                                Disconnect(DiscordMediaCloseCode.InvalidEncryptionMode, "Unexpected encryption mode: " + description.EncryptionMode);
                            else
                            {
                                SecretKey = description.SecretKey;

                                State = MediaSessionState.Connected;

                                Task.Run(() => HandleConnect());

                                if (_heldBackMessages.Count > 0)
                                {
                                    Task.Run(() =>
                                    {
                                        foreach (var msg in _heldBackMessages)
                                            WebSocket.Send(msg.Key, msg.Value);

                                        _heldBackMessages.Clear();
                                    });
                                }
                            }
                        }
                        break;
                    case DiscordMediaOpcode.ChangeCodecs:
                        // apparently this triggers whenever u switch channel
                        // im confused
                        //var codecs = message.Data.ToObject<MediaCodecSelection>();
                        break;
                    case DiscordMediaOpcode.Hello:
                        WebSocket.Send(DiscordMediaOpcode.Identify, new DiscordMediaIdentify()
                        {
                            ServerId = GetServerId(),
                            UserId = Client.User.Id,
                            SessionId = Client.SessionId,
                            Token = Server.Token,
                            Video = true
                        });

                        Task.Run(() =>
                        {
                            try
                            {
                                while (true)
                                {
                                    WebSocket.Send(DiscordMediaOpcode.Heartbeat, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                                    Thread.Sleep(message.Data.Value<int>("heartbeat_interval"));
                                }
                            }
                            catch { }
                        });

                        break;
                    default:
                        HandleMessage(message);
                        break;
                }
            }
            catch (InvalidOperationException) { }
        }


        /// <summary>
        /// Disconnects from the current voice channel
        /// </summary>
        public virtual void Disconnect()
        {
            Disconnect(DiscordMediaCloseCode.ClosedByClient, "Closed by client");
        }

        internal void Disconnect(DiscordMediaCloseCode error, string reason) 
        {
            Disconnect((ushort)error, reason);
        }

        internal void Disconnect(ushort error, string reason)
        {
            if (WebSocket != null && State > MediaSessionState.NotConnected)
                WebSocket.Close(error, reason);
        }


        public void SetSSRC(uint audioSsrc)
        {
            SSRC = new DiscordSSRC() { Audio = audioSsrc, Video = audioSsrc + 1, Rtx = audioSsrc + 2 };

            SafeSend(DiscordMediaOpcode.SSRCUpdate, SSRC);
        }


        protected abstract ulong GetServerId();

        protected virtual void HandleMessage(DiscordWebSocketMessage<DiscordMediaOpcode> message) { }
        protected virtual void HandlePacket(RTPPacketHeader header, byte[] payload) { }

        protected abstract void HandleConnect();
        protected abstract void HandleDisconnect(DiscordMediaCloseEventArgs args);


        private bool ValidateCodecs(MediaCodecSelection codecs)
        {
            string invalidCodec = null;

            if (!SupportedCodecs.ContainsKey(codecs.AudioCodec))
                invalidCodec = codecs.AudioCodec;
            // we can ignore the video codec since we don't do anything with the data right now anyway
            /*else if (!SupportedCodecs.ContainsKey(codecs.VideoCodec))
                invalidCodec = codecs.VideoCodec;*/

            if (invalidCodec != null)
            {
                Disconnect(DiscordMediaCloseCode.UnknownCodec, "Unexpected codec: " + invalidCodec);

                return false;
            }
            else
                return true;
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
            WebSocket.Send(DiscordMediaOpcode.SelectProtocol, new MediaProtocolSelection()
            {
                Protocol = "udp",
                ProtocolData = new MediaProtocolData()
                {
                    Host = _localEndpoint.Address.ToString(),
                    Port = _localEndpoint.Port,
                    EncryptionMode = Sodium.EncryptionMode
                },
                RtcConnectionId = Guid.NewGuid().ToString(),
                Codecs = SupportedCodecs.Values.ToList()
            });
        }


        private void StartListener()
        {
            try
            {
                while (true)
                {
                    byte[] received = UdpClient.Receive(ref _localEndpoint);

                    if (BitConverter.ToInt16(new byte[] { received[1], received[0] }, 0) == 2)
                    {
                        string ip = "";
                        for (int i = 8; i < received.Length; i++)
                        {
                            if (received[i] == 0)
                                break;
                            else
                                ip += (char)received[i];
                        }

                        _localEndpoint = new IPEndPoint(IPAddress.Parse(ip), BitConverter.ToUInt16(new byte[] { received[received.Length - 1], received[received.Length - 2] }, 0));

                        SelectProtocol();
                    }
                    else if (ReceivePackets && received[0] == 0x80 || received[0] == 0x90)
                    {
                        while (SecretKey == null) { Thread.Sleep(100); }

                        try
                        {
                            var header = RTPPacketHeader.Read(SecretKey, received, out byte[] payload);

                            HandlePacket(header, payload);
                        }
                        catch (SodiumException) { }
                    }
                }
            }
            catch { }
        }

        public void Dispose()
        {
            if (State > MediaSessionState.NotConnected)
                Disconnect();

            if (WebSocket != null)
            {
                WebSocket.Dispose();
                WebSocket = null;
            }

            if (UdpClient != null)
                UdpClient.Close();

            _serverEndpoint = null;
            _localEndpoint = null;

            _heldBackMessages.Clear();
            _heldBackMessages = null;

            SecretKey = null;
        }
    }
}

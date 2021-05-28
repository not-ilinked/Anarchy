using Discord.Gateway;
using Discord.WebSockets;
using System;
using System.Collections.Generic;
using System.IO;
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

        internal MediaWebSocket WebSocket { get; private set; }
        public DiscordSocketClient Client { get; private set; }

        public IPEndPoint ServerEndpoint { get; private set; }
        private IPEndPoint _localEndpoint;
        private List<KeyValuePair<DiscordMediaOpcode, object>> _heldBackMessages;

        internal UdpClient UdpClient { get; set; }
        public DiscordSSRC SSRC { get; set; }
        internal byte[] SecretKey { get; private set; }
        internal string SessionId { get; private set; }
        internal static readonly Dictionary<string, MediaCodec> SupportedCodecs = new Dictionary<string, MediaCodec>()
        {
            { "opus", new MediaCodec() { Name = "opus", Type = CodecType.Audio, PayloadType = 120, Priority = 1000 } },
            { "H264", new VideoMediaCodec() { Name = "H264", Type = CodecType.Video, PayloadType = 101, Priority = 1000, RtxPayloadType = 102 } }
        };

        public DiscordMediaServer CurrentServer { get; internal set; }
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

        internal object _fileLock = new object();

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
            WebSocket = other.WebSocket;
            WebSocket.OnClosed += OnClosed;
            WebSocket.OnMessageReceived += OnMessageReceived;
            other.State = MediaSessionState.Dead;
            other.OnServerUpdated = null;
            other.WebSocket.OnMessageReceived -= OnMessageReceived;
            other.WebSocket.OnClosed -= OnClosed;
            ServerEndpoint = other.ServerEndpoint;
            _localEndpoint = other._localEndpoint;
            UdpClient = other.UdpClient;
            SSRC = other.SSRC;
            SecretKey = other.SecretKey;
            SessionId = other.SessionId;
            CurrentServer = other.CurrentServer;
            ReceivePackets = other.ReceivePackets;
            State = other.State;   
        }


        public void Connect()
        {
            if (State == MediaSessionState.Authenticated)
                Task.Run(() => HandleConnect());
            else
            {
                State = MediaSessionState.Connecting;

                WebSocket?.Close((ushort)UnorthodoxCloseCode.KeepQuiet, "Killing old websocket");

                WebSocket = new MediaWebSocket("wss://" + CurrentServer.Endpoint + "?v=4", this);
                WebSocket.OnClosed += OnClosed;
                WebSocket.OnMessageReceived += OnMessageReceived;
                WebSocket.SetProxy(Client.Proxy);
                WebSocket.Connect();
            }
        }


        internal void Send<T>(DiscordMediaOpcode op, T data)
        {
            if (State == MediaSessionState.Authenticated)
                WebSocket.Send(op, data);
            else
                throw new InvalidOperationException("Connection state must be Authenticated");
        }


        internal void UpdateServer(DiscordMediaServer server)
        {
            CurrentServer = server;
            State = MediaSessionState.StandBy;

            if (server.Endpoint != null && WebSocket != null)
                Task.Run(() => Connect());

            if (OnServerUpdated != null)
                Task.Run(() => OnServerUpdated.Invoke(this, server));
        }


        protected bool JustifyThread(int expectedId)
        {
            return WebSocket != null && State > MediaSessionState.Dead && WebSocket.Id == expectedId;
        }


        private void OnClosed(object sender, CloseEventArgs args)
        {
            State = MediaSessionState.StandBy;

            if (args.Code == 1006)
                Thread.Sleep(200);

            // so far i've only seen code 1000 being used because of an invalid session but still gonna reconnect to be safe
            var code = (DiscordMediaCloseCode)args.Code;
            if (args.Code == 1000 || args.Code == 1006 || code == DiscordMediaCloseCode.SessionTimeout || code == DiscordMediaCloseCode.ServerCrashed)
                Task.Run(() => Connect());
            else
            {
                if (args.Code != (ushort)UnorthodoxCloseCode.KeepQuiet)
                    Task.Run(() => HandleDisconnect(new DiscordMediaCloseEventArgs(code, args.Reason)));
            }
        }

        private void OnMessageReceived(object sender, DiscordWebSocketMessage<DiscordMediaOpcode> message)
        {
            try
            {
                switch (message.Opcode)
                {
                    case DiscordMediaOpcode.Ready:
                        DiscordMediaReady ready = message.Data.ToObject<DiscordMediaReady>();

                        SSRC = new DiscordSSRC() { Audio = ready.SSRC, Video = ready.Streams != null ? ready.Streams[0].SSRC : 0 };

                        ServerEndpoint = new IPEndPoint(IPAddress.Parse(ready.IP), ready.Port);

                        UdpClient = new UdpClient();
                        UdpClient.Connect(ServerEndpoint);

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
                                State = MediaSessionState.Authenticated;

                                Task.Run(() => HandleConnect());
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
                            Token = CurrentServer.Token,
                            Video = true
                        });

                        WebSocket.StartHeartbeaterAsync(message.Data.Value<int>("heartbeat_interval"));
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
            if (WebSocket != null && State > MediaSessionState.StandBy)
                WebSocket.Close(error, reason);
        }


        public void SetSSRC(uint audioSsrc)
        {
            SSRC = new DiscordSSRC() { Audio = audioSsrc, Video = audioSsrc + 1, Rtx = audioSsrc + 2 };

            Send(DiscordMediaOpcode.SSRCUpdate, SSRC);
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


        private void StartListener()
        {
            int id = WebSocket.Id;
            var client = UdpClient;

            try
            {
                while (JustifyThread(id))
                {
                    byte[] received = client.Receive(ref _localEndpoint);

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

                        WebSocket.SelectProtocol(_localEndpoint);
                    }
                    else if (received[0] == 0x80 || received[0] == 0x90)
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

            client?.Close();
        }

        public void Dispose()
        {
            Disconnect();

            if (WebSocket != null)
            {
                WebSocket.Dispose();
                WebSocket = null;
            }

            if (UdpClient != null)
                UdpClient.Close();

            ServerEndpoint = null;
            _localEndpoint = null;

            _heldBackMessages.Clear();
            _heldBackMessages = null;

            SecretKey = null;
        }
    }
}

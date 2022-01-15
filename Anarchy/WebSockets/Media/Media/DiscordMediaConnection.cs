﻿using Discord.Gateway;
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
    internal class DiscordMediaConnection : DiscordWebSocket<DiscordMediaOpcode>
    {
        public delegate void WSMessageHandler(DiscordMediaConnection connection, DiscordWebSocketMessage<DiscordMediaOpcode> message);
        public event WSMessageHandler OnMessage;

        public delegate void PacketHandler(DiscordMediaConnection connection, MediaPacketEventArgs args);
        public event PacketHandler OnUdpPacket;

        public delegate void ReadyHandler(DiscordMediaConnection connection);
        public event ReadyHandler OnReady;

        public delegate void KillHandler(DiscordMediaConnection connection, CloseEventArgs args);
        public event KillHandler OnDead;

        internal static readonly Dictionary<string, MediaCodec> SupportedCodecs = new Dictionary<string, MediaCodec>()
        {
            { "opus", new MediaCodec() { Name = "opus", Type = CodecType.Audio, PayloadType = 120, Priority = 1000 } },
            { "H264", new VideoMediaCodec() { Name = "H264", Type = CodecType.Video, PayloadType = 101, Priority = 1000, RtxPayloadType = 102 } }
        };

        public MediaConnectionState State { get; private set; }

        private readonly ulong _serverId;

        private readonly DiscordMediaServer _server;

        public DiscordSSRC SSRC { get; private set; }
        public byte[] SecretKey { get; set; }

        internal UdpClient UdpClient { get; private set; }
        public IPEndPoint ServerEndpoint { get; private set; }
        private IPEndPoint _localEndpoint;

        private readonly DiscordSocketClient _parentClient;

        public DiscordMediaConnection(DiscordSocketClient parentClient, ulong serverId, DiscordMediaServer server) : base("wss://" + server.Endpoint + "?v=4")
        {
            _parentClient = parentClient;

            _server = server;
            _serverId = serverId;

            OnMessageReceived += HandleMessage;
            OnClosed += HandleClose;
        }

        private void HandleClose(object sender, CloseEventArgs args)
        {
            State = MediaConnectionState.NotConnected;

            if (args.Code == 1006)
            {
                Thread.Sleep(200);
                Task.Run(() => Connect());
                return;
            }
            else if (args.Code >= 4000)
            {
                DiscordMediaCloseCode discordCode = (DiscordMediaCloseCode)args.Code;

                if (discordCode == DiscordMediaCloseCode.SessionTimeout || discordCode == DiscordMediaCloseCode.ServerCrashed)
                {
                    Task.Run(() => Connect());
                    return;
                }
            }

            if (args.Code != 1004)
            {
                OnDead?.Invoke(this, args);
            }
        }

        private void HandleMessage(object sender, DiscordWebSocketMessage<DiscordMediaOpcode> message)
        {
            switch (message.Opcode)
            {
                case DiscordMediaOpcode.Ready:
                    DiscordMediaReady ready = message.Data.ToObject<DiscordMediaReady>();

                    SSRC = new DiscordSSRC() { Audio = ready.SSRC };

                    ServerEndpoint = new IPEndPoint(IPAddress.Parse(ready.IP), ready.Port);

                    UdpClient = new UdpClient();
                    UdpClient.Connect(ServerEndpoint);

                    Task.Run(() => StartListener());

                    Holepunch();
                    break;
                case DiscordMediaOpcode.SessionDescription:
                    DiscordSessionDescription description = message.Data.ToObject<DiscordSessionDescription>();

                    SecretKey = description.SecretKey;

                    State = MediaConnectionState.Ready;
                    OnReady?.Invoke(this);
                    break;
                case DiscordMediaOpcode.Hello:
                    Send(DiscordMediaOpcode.Identify, new DiscordMediaIdentify()
                    {
                        ServerId = _serverId,
                        UserId = _parentClient.User.Id,
                        SessionId = _parentClient.SessionId,
                        Token = _server.Token,
                        Video = true
                    });

                    StartHeartbeaterAsync(message.Data.Value<int>("heartbeat_interval"));
                    break;
                default:
                    OnMessage?.Invoke(this, message);
                    break;
            }
        }

        public new void Connect()
        {
            State = MediaConnectionState.Connecting;
            base.Connect();
        }


        public void SetSSRC(uint audioSsrc)
        {
            SSRC = new DiscordSSRC() { Audio = audioSsrc, Video = audioSsrc + 1, Rtx = audioSsrc + 2 };
            Send(DiscordMediaOpcode.SSRCUpdate, SSRC);
        }


        private async void StartHeartbeaterAsync(int interval)
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


        private void SelectProtocol(IPEndPoint localEndpoint)
        {
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
                Codecs = SupportedCodecs.Values.ToList()
            });
        }


        private void StartListener()
        {
            try
            {
                while (State > MediaConnectionState.NotConnected)
                {
                    byte[] received = UdpClient.Receive(ref _localEndpoint);

                    if (BitConverter.ToInt16(new byte[] { received[1], received[0] }, 0) == 2)
                    {
                        string ip = "";
                        for (int i = 8; i < received.Length; i++)
                        {
                            if (received[i] == 0)
                            {
                                break;
                            }
                            else
                            {
                                ip += (char)received[i];
                            }
                        }

                        _localEndpoint = new IPEndPoint(IPAddress.Parse(ip), BitConverter.ToUInt16(new byte[] { received[received.Length - 1], received[received.Length - 2] }, 0));

                        SelectProtocol(_localEndpoint);
                    }
                    else if (received[0] == 0x80 || received[0] == 0x90)
                    {
                        while (SecretKey == null) { Thread.Sleep(100); }

                        //Console.WriteLine($"{received[0]} {received[1]} {received[2]} {received[3]} {received[4]} {received[5]} {received[6]} {received[7]} {received[8]} {received[9]} {received[10]} {received[11]}");
                        /*
                        var ok = RTPPacketHeader.Read(SecretKey, received, out var _);

                        Console.WriteLine($"{ok.Type} {ok.Flags} {ok.Sequence} {ok.Timestamp} {ok.SSRC} {ok.HasExtensions}");
                        */
                        // not much point in doing this rn since the decryption fails

                        if (_parentClient.Config.ParseIncomingRTPData)
                        {
                            try
                            {
                                RTPPacketHeader header = RTPPacketHeader.Read(SecretKey, received, out byte[] payload);

                                OnUdpPacket?.Invoke(this, new MediaPacketEventArgs(header, payload));
                            }
                            catch (SodiumException) { }
                        }
                    }
                }
            }
            catch { }

            UdpClient.Close();
        }
    }
}

using Discord.Gateway;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using WebSocketSharp;
using System.Threading.Tasks;
using System.Net.Sockets;
using Leaf.xNet;

namespace Discord.Voice
{
    public class DiscordVoiceSession
    {
        private readonly WebSocket _socket;
        private readonly DiscordSocketClient _client;

        internal UdpClient UdpClient { get; set; }
        internal ushort Sequence { get; set; }
        internal uint Timestamp { get; set; }
        internal int SSRC { get; private set; }
        internal byte[] SecretKey { get; private set; }
        internal object VoiceLock { get; private set; }

        public DiscordVoiceServer Server { get; private set; }
        public ulong ChannelId { get; private set; }

        public DiscordVoiceClientState State { get; private set; }
        public bool Speaking { get; private set; }


        public delegate void ConnectHandler(DiscordVoiceSession session, EventArgs e);
        public event ConnectHandler OnConnected;

        public delegate void DisconnectHandler(DiscordVoiceSession session, DiscordVoiceCloseEventArgs error);
        public event DisconnectHandler OnDisconnected;

        public delegate void SpeakingHandler(DiscordVoiceSession session, DiscordVoiceSpeaking speaking);
        public event SpeakingHandler OnUserSpeaking;


        public DiscordVoiceSession(DiscordSocketClient client, DiscordVoiceServer server, ulong channelId)
        {
            VoiceLock = new object();
            _client = client;
            Server = server;
            ChannelId = channelId;

            _socket = new WebSocket("wss://" + Server.Server.Split(':')[0] + "?v=4");
            UdpClient = new UdpClient();

            if (_client.Config.Proxy != null)
            {
                if (_client.Config.Proxy.Type == ProxyType.HTTP) //WebSocketSharp only supports HTTP proxies :(
                    _socket.SetProxy("http://" + _client.Config.Proxy, "", "");
            }

            _socket.OnClose += (sender, e) =>
            {
                DiscordVoiceCloseEventArgs error = null;
                if (e.Code > 1000)
                    error = new DiscordVoiceCloseEventArgs((DiscordVoiceCloseError)e.Code, e.Reason);

                State = DiscordVoiceClientState.NotConnected;

                OnDisconnected?.Invoke(this, error);
            };

            _socket.OnMessage += Socket_OnMessage;

            _socket.Connect();

            State = DiscordVoiceClientState.Connecting;
        }


        internal void SendSocketData<T>(DiscordVoiceOpcode op, T payload)
        {
            _socket.Send(JsonConvert.SerializeObject(new DiscordVoiceRequest<T>()
            {
                Opcode = op,
                Payload = payload
            }));
        }


        /// <summary>
        /// Disconnects from the current voice channel
        /// </summary>
        public void Disconnect()
        {
            try
            {
                _client.ChangeVoiceState(new VoiceStateChange() { GuildId = Server.Guild == null ? null : (ulong?)Server.Guild.Id, ChannelId = null });
            }
            catch { }

            _socket.Close();

            UdpClient.Close();
        }


        /// <summary>
        /// Tells Discord whether you're going to send data or not.
        /// This automatically gets set to true whenever there is being sent data through a <see cref="DiscordVoiceStream"/>. 
        /// You will however need to set it to false once you won't be sending any more data through.
        /// </summary>
        /// <param name="speaking">Whether there will be getting sent data through or not</param>
        public void SetSpeaking(bool speaking)
        {
            if (State != DiscordVoiceClientState.Connected)
                throw new InvalidOperationException("Connection has been closed.");

            SendSocketData(DiscordVoiceOpcode.Speaking, new DiscordSpeakingRequest()
            {
                State = speaking ? DiscordVoiceSpeakingState.Microphone : DiscordVoiceSpeakingState.NotSpeaking,
                Delay = 0,
                SSRC = SSRC
            });

            Speaking = speaking;
        }


        /// <summary>
        /// Creates a stream to which you can write audio data.
        /// </summary>
        /// <param name="application">What the encoding should be optimized for</param>
        public DiscordVoiceStream CreateStream(uint bitrate, AudioApplication application = AudioApplication.Mixed)
        {
            if (State != DiscordVoiceClientState.Connected)
                throw new InvalidOperationException("Connection has been closed.");

            return new DiscordVoiceStream(this, (int)bitrate, application);
        }


        private void Socket_OnMessage(object sender, WebSocketSharp.MessageEventArgs e)
        {
            var payload = e.Data.Deserialize<DiscordVoiceResponse>();

            Task.Run(() =>
            {
                switch (payload.Opcode)
                {
                    case DiscordVoiceOpcode.Ready:
                        DiscordVoiceReady ready = payload.Deserialize<DiscordVoiceReady>();

                        SSRC = ready.SSRC;

                        SendSocketData(DiscordVoiceOpcode.SelectProtocol, new DiscordVoiceProtocolSelection()
                        {
                            Protocol = "udp",
                            ProtocolData = new DiscordVoiceProtocolData()
                            {
                                Host = ready.IP,
                                Port = ready.Port,
                                EncryptionMode = "xsalsa20_poly1305"
                            }
                        });

                        UdpClient.Connect(ready.IP, ready.Port);
                        break;
                    case DiscordVoiceOpcode.Speaking:
                        OnUserSpeaking?.Invoke(this, payload.Deserialize<DiscordVoiceSpeaking>());
                        break;
                    case DiscordVoiceOpcode.SessionDescription:
                        List<byte> why = new List<byte>();

                        foreach (byte item in payload.Deserialize<dynamic>().secret_key)
                            why.Add(item);

                        SecretKey = why.ToArray();

                        State = DiscordVoiceClientState.Connected;

                        OnConnected?.Invoke(this, null);

                        StartSpeakAsync();
                        break;
                    case DiscordVoiceOpcode.Hello:
                        var ident = new DiscordVoiceIdentify()
                        {
                            GuildId = Server.Guild == null ? ChannelId : Server.Guild.Id,
                            UserId = _client.User.Id,
                            SessionId = _client.SessionId,
                            Token = Server.Token
                        };

                        SendSocketData(DiscordVoiceOpcode.Identify, ident);

                        try
                        {
                            while (true)
                            {
                                SendSocketData(DiscordVoiceOpcode.Heartbeat, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                                Thread.Sleep((int)payload.Deserialize<dynamic>().heartbeat_interval);
                            }
                        }
                        catch { }

                        break;
                }
            });
        }

        private async void StartSpeakAsync()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    // TODO: figure out a way to detect when the user has stopped speaking
                }
            });
        }
    }
}

using Leaf.xNet;
using Newtonsoft.Json;
using System;
using WebSocketSharp;

namespace Discord.WebSockets
{
    public class DiscordWebSocket<TOpcode> : IDisposable where TOpcode : Enum
    {
        private WebSocket _socket;

        public delegate void MessageHandler(object sender, DiscordWebSocketMessage<TOpcode> message);
        public event MessageHandler OnMessageReceived;

        public delegate void CloseHandler(object sender, CloseEventArgs args);
        public event CloseHandler OnClosed;

        public DiscordWebSocket(string url)
        {
            _socket = new WebSocket(url)
            {
                Origin = "https://discordapp.com"
            };
            _socket.OnMessage += OnMessage;
            _socket.OnClose += OnClose;
        }

        public void SetProxy(ProxyClient client)
        {
            if (client != null && client.Type == ProxyType.HTTP)
                _socket.SetProxy($"http://{client.Host}:{client.Port}", client.Username, client.Password);
        }

        public void Connect()
        {
            _socket.Connect();
        }

        public void Close(ushort error, string reason)
        {
            _socket.Close(error, reason);
        }

        public void Send<T>(TOpcode op, T data)
        {
            _socket.Send(JsonConvert.SerializeObject(new DiscordWebSocketRequest<T, TOpcode>(op, data)));
        }

        private void OnClose(object sender, CloseEventArgs e)
        {
            OnClosed?.Invoke(this, e);
        }

        private void OnMessage(object sender, WebSocketSharp.MessageEventArgs e)
        {
            OnMessageReceived?.Invoke(this, e.Data.Deserialize<DiscordWebSocketMessage<TOpcode>>());
        }

        public void Dispose()
        {
            _socket = null;
        }
    }
}

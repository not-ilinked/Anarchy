using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Discord.WebSockets
{
    public class DiscordWebSocket<TOpcode> : IWebSocketClient<TOpcode> where TOpcode : Enum
    {
        private const int OutgoingChunkSize = 8192; // 8 KiB
        private const int IncomingChunkSize = 32768; // 32 KiB

        /// <inheritdoc />
        public IWebProxy Proxy { get; }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> DefaultHeaders { get; }
        private readonly Dictionary<string, string> _defaultHeaders;

        private readonly Uri _uri;

        private Task<DiscordWebSocketCloseEventArgs> _receiverTask;
        private CancellationTokenSource _receiverTokenSource;
        private CancellationToken _receiverToken;
        private readonly SemaphoreSlim _senderLock;

        private CancellationTokenSource _socketTokenSource;
        private CancellationToken _socketToken;

        private ClientWebSocket _ws;

        private volatile bool _isConnected = false;
        private bool _isDisposed = false;

        /// <summary>
        /// Triggered when the client is disconnected.
        /// </summary>
        public event EventHandler<DiscordWebSocketCloseEventArgs> OnClosed;

        /// <summary>
        /// Triggered when the client receives a message from the remote party.
        /// </summary>
        public event EventHandler<DiscordWebSocketMessage<TOpcode>> OnMessageReceived;

        /// <summary>
        /// Instantiates a new WebSocket client with specified proxy settings.
        /// </summary>
        /// <param name="proxy">Proxy settings for the client.</param>
        protected DiscordWebSocket(string url = null, IWebProxy proxy = null)
        {
            _uri = url != null ? new Uri(url) : new Uri("https://discord.com");

            Proxy = proxy;
            _defaultHeaders = new Dictionary<string, string>();
            DefaultHeaders = new ReadOnlyDictionary<string, string>(_defaultHeaders);

            _receiverTokenSource = null;
            _receiverToken = CancellationToken.None;
            _senderLock = new SemaphoreSlim(1);

            _socketTokenSource = null;
            _socketToken = CancellationToken.None;
        }

        /// <inheritdoc />
        public async Task ConnectAsync()
        {
            // Disconnect first
            try { await DisconnectAsync().ConfigureAwait(false); } catch { }

            // Disallow sending messages
            await _senderLock.WaitAsync().ConfigureAwait(false);

            try
            {
                // This can be null at this point
                _receiverTokenSource?.Dispose();
                _socketTokenSource?.Dispose();

                _ws?.Dispose();
                _ws = new ClientWebSocket();
                _ws.Options.Proxy = Proxy;
                _ws.Options.KeepAliveInterval = TimeSpan.Zero;
                if (_defaultHeaders != null)
                    foreach (var (k, v) in _defaultHeaders)
                        _ws.Options.SetRequestHeader(k, v);

                _receiverTokenSource = new CancellationTokenSource();
                _receiverToken = _receiverTokenSource.Token;

                _socketTokenSource = new CancellationTokenSource();
                _socketToken = _socketTokenSource.Token;

                _isDisposed = false;
                await _ws.ConnectAsync(_uri, _socketToken).ConfigureAwait(false);
                _receiverTask = Task.Run(ReceiverLoopAsync, CancellationToken.None);
                _ = _receiverTask.ContinueWith(
                    task => { OnClosed.Invoke(this, task.Result); },
                    TaskContinuationOptions.OnlyOnRanToCompletion
                );
            }
            finally
            {
                _senderLock.Release();
            }
        }

        /// <inheritdoc />
        public async Task DisconnectAsync(int code = 1000, string message = "")
        {
            // Ensure that messages cannot be sent
            await _senderLock.WaitAsync().ConfigureAwait(false);

            try
            {
                if (_ws != null && (_ws.State == WebSocketState.Open || _ws.State == WebSocketState.CloseReceived))
                    await _ws.CloseOutputAsync((WebSocketCloseStatus) code, message, CancellationToken.None).ConfigureAwait(false);

                if (_receiverTask != null)
                    await _receiverTask.ConfigureAwait(false); // Ensure that receiving completed

                _isConnected = false;

                if (!_isDisposed)
                {
                    // Cancel all running tasks
                    if (_socketToken.CanBeCanceled)
                        _socketTokenSource?.Cancel();
                    _socketTokenSource?.Dispose();

                    if (_receiverToken.CanBeCanceled)
                        _receiverTokenSource?.Cancel();
                    _receiverTokenSource?.Dispose();

                    _isDisposed = true;
                }
            }
            finally
            {
                _senderLock.Release();
            }
        }

        public void SendMessage<T>(TOpcode op, T data)
        {
            var message = JsonConvert.SerializeObject(new DiscordWebSocketRequest<T, TOpcode>(op, data));
            SendMessageAsync(message).GetAwaiter().GetResult();
        }

        /// <inheritdoc />
        public async Task SendMessageAsync(string message)
        {
            if (_ws == null)
                return;

            await _senderLock.WaitAsync().ConfigureAwait(false);

            try
            {
                if (_ws.State != WebSocketState.Open && _ws.State != WebSocketState.CloseReceived)
                    return;

                var bytes = new UTF8Encoding(false).GetBytes(message);

                var len = bytes.Length;
                var segCount = len / OutgoingChunkSize;
                if (len % OutgoingChunkSize != 0)
                    segCount++;

                for (var i = 0; i < segCount; i++)
                {
                    var segStart = OutgoingChunkSize * i;
                    var segLen = Math.Min(OutgoingChunkSize, len - segStart);

                    await _ws.SendAsync(new ArraySegment<byte>(bytes, segStart, segLen), WebSocketMessageType.Text, i == segCount - 1, CancellationToken.None).ConfigureAwait(false);
                }
            }
            finally
            {
                _senderLock.Release();
            }
        }

        /// <inheritdoc />
        public bool AddDefaultHeader(string name, string value)
        {
            _defaultHeaders[name] = value;
            return true;
        }

        /// <inheritdoc />
        public bool RemoveDefaultHeader(string name)
            => _defaultHeaders.Remove(name);

        /// <summary>
        /// Disposes of resources used by this WebSocket client instance.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            DisconnectAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            GC.SuppressFinalize(this);
        }

        internal async Task<DiscordWebSocketCloseEventArgs> ReceiverLoopAsync()
        {
            await Task.Yield();

            var buffer = new ArraySegment<byte>(new byte[IncomingChunkSize]);

            try
            {
                using var bs = new MemoryStream();
                while (!_receiverToken.IsCancellationRequested)
                {
                    // See https://github.com/RogueException/Discord.Net/commit/ac389f5f6823e3a720aedd81b7805adbdd78b66d 
                    // for explanation on the cancellation token

                    WebSocketReceiveResult result;
                    byte[] resultBytes;
                    do
                    {
                        result = await _ws.ReceiveAsync(buffer, _receiverToken).ConfigureAwait(false);

                        if (_receiverToken.IsCancellationRequested)
                            break;

                        if (result.MessageType == WebSocketMessageType.Close)
                            break;

                        bs.Write(buffer.Array, 0, result.Count);
                    }
                    while (!result.EndOfMessage);

                    resultBytes = new byte[bs.Length];
                    bs.Position = 0;
                    bs.Read(resultBytes, 0, resultBytes.Length);
                    bs.Position = 0;
                    bs.SetLength(0);

                    if (!_isConnected && result.MessageType != WebSocketMessageType.Close)
                    {
                        _isConnected = true;
                        // An OnConnected event could be raised here. Currently there seems to be no need.
                    }

                    if (result.MessageType == WebSocketMessageType.Binary)
                    {
                        // Anarchy currently only supports plain text messages.
                    }

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        OnMessageReceived.Invoke(this,
                            JsonConvert.DeserializeObject<DiscordWebSocketMessage<TOpcode>>(Encoding.UTF8.GetString(resultBytes, 0, resultBytes.Length))
                        );
                    }

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        var code = (int) result.CloseStatus.Value;
                        var message = result.CloseStatusDescription;

                        if (_ws.State == WebSocketState.Open || _ws.State == WebSocketState.CloseReceived)
                            await _ws.CloseOutputAsync((WebSocketCloseStatus) code, message, CancellationToken.None).ConfigureAwait(false);

                        return new DiscordWebSocketCloseEventArgs(code, message);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is WebSocketException)
                    return new DiscordWebSocketCloseEventArgs(1006, ex.Message);
                else
                    return new DiscordWebSocketCloseEventArgs(-1, String.Empty);
            }

            return new DiscordWebSocketCloseEventArgs((int) WebSocketCloseStatus.NormalClosure, "Cancellation Requested");
        }

        /// <summary>
        /// Creates a new instance of <see cref="DiscordWebSocket"/>.
        /// </summary>
        /// <param name="proxy">Proxy to use for this client instance.</param>
        /// <returns>An instance of <see cref="DiscordWebSocket"/>.</returns>
        public static IWebSocketClient<TOpcode> CreateNew(string url, IWebProxy proxy = null)
            => new DiscordWebSocket<TOpcode>(url, proxy);
    }
}

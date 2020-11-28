using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Discord.Media
{
    public class IncomingVoiceStream
    {
        public DiscordVoiceSession Session { get; private set; }
        public ulong UserId { get; private set; }

        private readonly ConcurrentQueue<DiscordVoicePacket> _packets;

        private event EventHandler _newPacket;
        private event EventHandler _onClose;

        internal int SilenceFramesReceived { get; set; }

        public bool Closed { get; private set; }
        public int QueuedPackets
        {
            get { return _packets.Count; }
        }

        public IncomingVoiceStream(DiscordVoiceSession session, ulong userId)
        {
            _packets = new ConcurrentQueue<DiscordVoicePacket>();
            Session = session;
            UserId = userId;
        }


        internal void Enqueue(DiscordVoicePacket packet)
        {
            SilenceFramesReceived = 0;
            _packets.Enqueue(packet);
            _newPacket?.Invoke(this, new EventArgs());
        }


        internal void Close()
        {
            if (!Closed)
            {
                Closed = true;
                _onClose?.Invoke(this, new EventArgs());
            }
        }


        public Task<DiscordVoicePacket> ReadAsync()
        {
            if (_packets.TryDequeue(out DiscordVoicePacket packet))
                return Task.FromResult(packet);
            else if (Session.State == MediaSessionState.Authenticated && !Closed)
            {
                TaskCompletionSource<DiscordVoicePacket> task = new TaskCompletionSource<DiscordVoicePacket>();

                async void handler(object sender, EventArgs e)
                {
                    _newPacket -= handler;
                    _onClose -= closeHandler;
                    task.SetResult(await ReadAsync());
                }

                void closeHandler(object sender, EventArgs e)
                {
                    _newPacket -= handler;
                    _onClose -= closeHandler;
                    task.SetException(new InvalidOperationException("The parent session or this receiver has been closed."));
                }

                _newPacket += handler;
                _onClose += closeHandler;

                return task.Task;
            }
            else
                throw new InvalidOperationException("The parent session or this receiver has been closed.");
        }

        public DiscordVoicePacket Read()
        {
            return ReadAsync().GetAwaiter().GetResult();
        }
    }
}

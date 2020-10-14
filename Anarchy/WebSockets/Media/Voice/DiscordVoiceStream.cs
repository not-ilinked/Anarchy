using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Discord.Media
{
    public class DiscordVoiceStream
    {
        public DiscordVoiceSession Session { get; private set; }
        private readonly OpusEncoder _encoder;
        private long _nextTick;

        internal DiscordVoiceStream(DiscordVoiceSession client, int bitrate, AudioApplication application = AudioApplication.Mixed)
        {
            Session = client;
            _encoder = new OpusEncoder(bitrate, application, 5);
            _nextTick = -1;
        }


        public int Write(byte[] buffer, int offset)
        {
            if (Session.State != MediaSessionState.Connected)
                throw new InvalidOperationException("Connection has been closed.");

            lock (Session.VoiceLock)
            {
                if (_nextTick == -1)
                    _nextTick = Environment.TickCount;
                else
                {
                    long distance = _nextTick - Environment.TickCount;

                    if (distance > 0)
                        Thread.Sleep((int)distance);
                }

                byte[] opusFrame = new byte[OpusEncoder.FrameBytes];
                int frameSize = _encoder.EncodeFrame(buffer, offset, opusFrame, 0);

                byte[] packet = new RTPPacketHeader()
                {
                    // Version = 0x80,
                    Type = DiscordMediaSession.SupportedCodecs["opus"].PayloadType,
                    Sequence = Session.Sequence,
                    Timestamp = Session.Timestamp,
                    SSRC = Session.SSRC.Audio
                }.Write(Session.SecretKey, opusFrame, 0, frameSize);

                Session.UdpClient.Send(packet, packet.Length);

                _nextTick += OpusEncoder.TimeBetweenFrames;
                Session.Sequence++;
                Session.Timestamp += OpusEncoder.FrameSamplesPerChannel;
            }
            
            return offset + OpusEncoder.FrameBytes;
        }

        [Obsolete("This overload is obsolete. pass a Stream instead")]
        public int CopyFrom(byte[] buffer, int offset = 0)
        {
            while (offset < buffer.Length)
            {
                try
                {
                    offset = Write(buffer, offset);
                }
                catch (SocketException)
                {
                    break;
                }
            }

            return offset;
        }

        public void CopyFrom(Stream stream)
        {
            if (!stream.CanRead)
                throw new ArgumentException("Cannot read from stream");

            byte[] buffer = new byte[OpusEncoder.FrameBytes];

            while (stream.Read(buffer, 0, buffer.Length) != 0)
                Write(buffer, 0);
        }
    }
}
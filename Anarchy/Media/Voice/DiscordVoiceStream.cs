using Discord.Media;
using System;
using System.Threading;

namespace Discord.Voice
{
    public class DiscordVoiceStream
    {
        public DiscordVoiceSession Session { get; private set; }
        private readonly OpusEncoder _encoder;
        private long _nextTick;

        internal DiscordVoiceStream(DiscordVoiceSession client, int bitrate, AudioApplication application = AudioApplication.Mixed)
        {
            Session = client;
            _encoder = new OpusEncoder(bitrate, application, 0);
            _nextTick = -1;
        }


        /// <summary>
        /// Sends audio data to the voice channel
        /// </summary>
        /// <param name="buffer">Audio data</param>
        /// <param name="offset">Offset to start from</param>
        /// <returns>The new offset, which u can use in your next Write call</returns>
        public int Write(byte[] buffer, int offset)
        {
            if (Session.State != DiscordMediaClientState.Connected)
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

                int length = new RTPPacketHeader() 
                {
                    // Version = 0x80,
                    Type = OpusEncoder.Codec.PayloadType,
                    Sequence = Session.Sequence,
                    Timestamp = Session.Timestamp,
                    SSRC = Session.SSRC.Audio
                }.Write(Session.SecretKey, opusFrame, 0, frameSize, out byte[] packet);

                Session.UdpClient.Send(packet, length);

                _nextTick += OpusEncoder.TimeBetweenFrames;
                Session.Sequence++;
                Session.Timestamp += OpusEncoder.FrameSamplesPerChannel;

                return offset + OpusEncoder.FrameBytes;
            }
        }


        /// <summary>
        /// Writes audio data to the voice channel
        /// </summary>
        /// <param name="buffer">Your audio data</param>
        /// <param name="offset">Offset to start from</param>
        /// <returns>Offset the copying stopped at. This will be less than buffer.Length if an error occured.</returns>
        public int CopyFrom(byte[] buffer, int offset = 0)
        {
            while (offset < buffer.Length)
            {
                try
                {
                    offset = Write(buffer, offset);
                }
                catch
                {
                    break;
                }
            }

            return offset;
        }

        public int CopyFrom(string filePath, int offset = 0)
        {
            return CopyFrom(DiscordVoiceUtils.ReadFromFile(filePath), offset);
        }
    }
}
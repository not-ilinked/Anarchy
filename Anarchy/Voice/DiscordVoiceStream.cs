using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Discord.Voice
{
    public class DiscordVoiceStream
    {
        private readonly DiscordVoiceSession _session;
        private readonly OpusEncoder _encoder;
        private long _nextTick;

        internal DiscordVoiceStream(DiscordVoiceSession client, int bitrate, AudioApplication application = AudioApplication.Mixed)
        {
            _session = client;
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
            if (_session.State != DiscordVoiceClientState.Connected)
                throw new InvalidOperationException("Connection has been closed.");

            lock (_session.VoiceLock)
            {
                if (_nextTick == -1)
                    _nextTick = Environment.TickCount;
                else
                {
                    long distance = _nextTick - Environment.TickCount;

                    if (distance > 0)
                        Thread.Sleep((int)distance);
                }

                byte[] packet = new byte[OpusEncoder.FrameBytes + 12];

                byte[] header = new byte[12];
                header[0] = 0x80;
                header[1] = 0x78;
                header[2] = (byte)(_session.Sequence >> 8);
                header[3] = (byte)(_session.Sequence >> 0);
                header[4] = (byte)(_session.Timestamp >> 24);
                header[5] = (byte)(_session.Timestamp >> 16);
                header[6] = (byte)(_session.Timestamp >> 8);
                header[7] = (byte)(_session.Timestamp >> 0);
                header[8] = (byte)(_session.SSRC >> 24);
                header[9] = (byte)(_session.SSRC >> 16);
                header[10] = (byte)(_session.SSRC >> 8);
                header[11] = (byte)(_session.SSRC >> 0);
                Buffer.BlockCopy(header, 0, packet, 0, 12);

                int frameSize = _encoder.EncodeFrame(buffer, offset, packet, 12);
                int encSize = Sodium.Encrypt(packet, 12, frameSize, packet, 12, header, _session.SecretKey);

                _session.SetSpeaking(true);
                _session.UdpClient.Send(packet, encSize + 12);


                _nextTick += OpusEncoder.TimeBetweenFrames;
                _session.Sequence++;
                _session.Timestamp += OpusEncoder.FrameSamplesPerChannel;

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
            while (offset < buffer.Length && _session.State == DiscordVoiceClientState.Connected)
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
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $"-hide_banner -loglevel panic -i \"{filePath}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });

            using (MemoryStream memStream = new MemoryStream())
            {
                process.StandardOutput.BaseStream.CopyTo(memStream);

                return CopyFrom(memStream.ToArray(), offset);
            }
        }
    }
}
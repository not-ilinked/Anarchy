using System;

namespace Discord.Media
{
    public class DiscordVoicePacket
    {
        public byte[] Data { get; private set; }
        public DateTime Timestamp { get; private set; }

        public DiscordVoicePacket(byte[] data)
        {
            Data = data;
            Timestamp = DateTime.Now;
        }
    }
}

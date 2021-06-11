using System;

namespace Discord.Media
{
    internal class MediaPacketEventArgs : EventArgs
    {
        public RTPPacketHeader Header { get; }
        public byte[] Payload { get; }

        public MediaPacketEventArgs(RTPPacketHeader header, byte[] payload)
        {
            Header = header;
            Payload = payload;
        }
    }
}

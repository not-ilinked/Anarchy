using System;

namespace Discord.Media
{
    internal static class H264Packager
    {
        public static VideoMediaCodec Codec { get; private set; } = new VideoMediaCodec() { Name = "H264", Type = CodecType.Video, Priority = 1000, PayloadType = 101, RtxPayloadType = 102 };

        // https://tools.ietf.org/html/rfc6184#section-1.3
        public static byte[] CreateNALUnit(byte nalHeader, byte[] payload, int offset, int count)
        {
            byte[] nalUnit = new byte[count + 1];
            nalUnit[0] = nalHeader;
            Buffer.BlockCopy(payload, offset, nalUnit, 1, count);

            return nalUnit;
        }
    }
}

using Discord.Voice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace Discord.Media
{
    /*
        Packet format:
        1 byte indicating the version (0x80)
        1 byte indicating the payload type (0x78/120 for Opus for an example)
        2 bytes for the sequence
        4 bytes for the timestamp
        4 bytes for the SSRC
        n bytes for the encrypted data
     */

    public class RTPPacketHeader
    {
        public RTPPacketHeader()
        {
            Extensions = new List<byte[]>();
        }

        public byte Version
        {
            get
            {
                return Extensions.Count > 0 ? (byte)0x90 : (byte)0x80;
            }
        }

        public byte Type { get; set; }
        public ushort Sequence { get; set; }
        public uint Timestamp { get; set; }
        public uint SSRC { get; set; }

        public List<byte[]> Extensions { get; private set; }

        private static readonly int HeaderLength = 12;

        public int Write(byte[] secretKey, byte[] buffer, int offset, int count, out byte[] packet)
        {
            packet = new byte[HeaderLength + count + Sodium.LengthDifference];

            byte[] header = new byte[HeaderLength];
            header[0] = Version;
            header[1] = Type;
            header[2] = (byte)(Sequence >> 8);
            header[3] = (byte)(Sequence >> 0);
            header[4] = (byte)(Timestamp >> 24);
            header[5] = (byte)(Timestamp >> 16);
            header[6] = (byte)(Timestamp >> 8);
            header[7] = (byte)(Timestamp >> 0);
            header[8] = (byte)(SSRC >> 24);
            header[9] = (byte)(SSRC >> 16);
            header[10] = (byte)(SSRC >> 8);
            header[11] = (byte)(SSRC >> 0);
            Buffer.BlockCopy(header, 0, packet, 0, HeaderLength);

            // don't think this is very necessary since we can just do packet.Length
            return HeaderLength + Sodium.Encrypt(buffer, offset, count, packet, HeaderLength, header, secretKey);
        }


        public static RTPPacketHeader Read(byte[] secretKey, byte[] packet, out byte[] payload)
        {
            byte[] rawHeader = new byte[HeaderLength];
            Buffer.BlockCopy(packet, 0, rawHeader, 0, rawHeader.Length);

            RTPPacketHeader header = new RTPPacketHeader()
            {
                // Version = packet[0],
                Type = packet[1],
                Sequence = BitConverter.ToUInt16(new byte[] { rawHeader[3], rawHeader[2] }, 0),
                Timestamp = BitConverter.ToUInt32(new byte[] { rawHeader[7], rawHeader[6], rawHeader[5], rawHeader[4] }, 0),
                SSRC = BitConverter.ToUInt32(new byte[] { rawHeader[11], rawHeader[10], rawHeader[9], rawHeader[8] }, 0)
            };

            byte[] decrypted = new byte[packet.Length - HeaderLength - Sodium.LengthDifference];

            Sodium.Decrypt(packet, HeaderLength, packet.Length - HeaderLength, decrypted, 0, rawHeader, secretKey);

            if (packet[0] == 0x90) // later on we might wanna check if index 3 (7 - 3 cuz big indian) is 1 to make anarchy's feature here live on for longer
            {
                ushort extensionCount = BitConverter.ToUInt16(new byte[] { decrypted[3], decrypted[2] }, 0);
                extensionCount++;

                const int extensionSize = 4;

                for (int i = 1; i < extensionCount; i++)
                {
                    byte[] extension = new byte[extensionSize];
                    Buffer.BlockCopy(decrypted, i * 4, extension, 0, extensionSize);

                    header.Extensions.Add(extension);
                }

                payload = new byte[decrypted.Length - extensionCount * extensionSize];
                Buffer.BlockCopy(decrypted, extensionCount * extensionSize, payload, 0, payload.Length);
            }
            else
                payload = decrypted;

            return header;
        }
    }
}

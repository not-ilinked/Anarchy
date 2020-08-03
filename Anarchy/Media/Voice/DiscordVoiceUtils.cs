using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Discord.Voice
{
    public static class DiscordVoiceUtils
    {
        public static byte[] ReadFromFile(string path)
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });

            using (MemoryStream memStream = new MemoryStream())
            {
                process.StandardOutput.BaseStream.CopyTo(memStream);

                return memStream.ToArray();
            }
        }

        private static List<int> FindNALUnitIndexes(byte[] byteStream)
        {
            List<int> indexes = new List<int>();

            byte[] startCode = new byte[] { 0, 0, 0, 1 };
            int currentStartCodeChar = 0;

            for (int i = 0; i < byteStream.Length; i++)
            {
                if (byteStream[i] == startCode[currentStartCodeChar])
                {
                    currentStartCodeChar++;

                    if (currentStartCodeChar == startCode.Length)
                        indexes.Add(i + 1);
                    else
                        continue; // don't wanna reset till we're done lel
                }
                
                currentStartCodeChar = 0;
            }

            return indexes;
        }

        public static byte[][] NALBytestreamToPackets(byte[] byteStream)
        {
            List<int> startIndexes = FindNALUnitIndexes(byteStream);

            byte[][] nalUnits = new byte[startIndexes.Count][];

            for (int i = 0; i < startIndexes.Count; i++)
            {
                int count;

                if (i == startIndexes.Count - 1)
                    count = byteStream.Length - startIndexes[i];
                else
                    count = startIndexes[i + 1] - startIndexes[i] - 4;
                
                nalUnits[i] = new byte[count];
                Buffer.BlockCopy(byteStream, startIndexes[i], nalUnits[i], 0, count);
            }

            return nalUnits;
        }
    }
}

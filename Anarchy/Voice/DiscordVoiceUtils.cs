using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                RedirectStandardOutput = true,
            });

            using (MemoryStream memStream = new MemoryStream())
            {
                process.StandardOutput.BaseStream.CopyTo(memStream);

                return memStream.ToArray();
            }
        }
    }
}

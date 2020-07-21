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

        /*
        public static void WriteToFile(byte[] data, string path)
        {
            var process = Process.Start(new ProcessStartInfo()
            {
                FileName = "ffmpeg.exe",
                Arguments = "-y -f s16le -r 48000 -ac 2 -i pipe:0 -vn -acodec libfdk_aac test.mp3",
                UseShellExecute = false,
                RedirectStandardInput = true
            });
            
            new MemoryStream(data).CopyTo(process.StandardInput.BaseStream);
            
            FileStream file = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);

            process.StandardOutput.BaseStream.CopyTo(file);

            file.Close();

            process.Close();
        }*/
    }
}

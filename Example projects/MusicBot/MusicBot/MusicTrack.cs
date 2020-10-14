using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace MusicBot
{
    public class MusicTrack
    {
        public Video Video { get; private set; }
        public string FilePath { get; private set; }
        public Task DownloadTask { get; private set; }

        public MusicTrack(string query)
        {
            Video = Program.YouTubeClient.Videos.GetAsync(query).Result;
            FilePath = "Tracks/" + Video.Id;
        }

        private async Task DownloadAsync()
        {
            var manifest = await Program.YouTubeClient.Videos.Streams.GetManifestAsync(Video.Id);

            var streamInfo = manifest.GetAudioOnly().WithHighestBitrate();
            await Program.YouTubeClient.Videos.Streams.DownloadAsync(streamInfo, FilePath);
        }

        public void StartDownload()
        {
            if (!File.Exists(FilePath))
                DownloadTask = DownloadAsync();
        }
    }
}

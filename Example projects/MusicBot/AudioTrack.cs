using System.Threading;

namespace MusicBot
{
    public class AudioTrack
    {
        public string Id { get; private set; }
        public string Title { get; private set; }
        public string ChannelName { get; private set; }

        public CancellationTokenSource CancellationTokenSource { get; private set; }

        public AudioTrack(string id)
        {
            Id = id;

            var video = Program.YouTubeClient.Videos.GetAsync(id).Result;
            Title = video.Title;
            ChannelName = video.Author.Title;

            CancellationTokenSource = new CancellationTokenSource();
        }
    }
}

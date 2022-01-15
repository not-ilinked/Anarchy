using Discord;
using Discord.Commands;

namespace MusicBot
{
    [Command("queue")]
    public class QueueCommand : CommandBase
    {
        [Parameter("action", true)]
        public string Action { get; private set; }

        public override void Execute()
        {
            if (Action == "clear")
            {
                if (Program.CanModifyList(Client, Message))
                {
                    TrackQueue list = Program.TrackLists[Message.Guild.Id];

                    AudioTrack currentSong = list.Tracks[0];
                    list.Tracks.Clear();

                    currentSong.CancellationTokenSource.Cancel();

                    Message.Channel.SendMessage("Queue has been cleared");
                }
            }
            else if (!Program.TrackLists.TryGetValue(Message.Guild.Id, out TrackQueue list) || list.Tracks.Count == 0)
            {
                Message.Channel.SendMessage("The music queue is empty");
            }
            else
            {
                EmbedMaker embed = new EmbedMaker() { Title = "Current queue" };
                foreach (AudioTrack song in list.Tracks)
                {
                    embed.AddField(song.Title, song.ChannelName + (song == list.Tracks[0] ? " *(Currently playing)*" : ""));
                }

                Message.Channel.SendMessage(embed);
            }
        }
    }
}

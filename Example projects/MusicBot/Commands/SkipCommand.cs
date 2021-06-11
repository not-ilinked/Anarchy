using Discord.Gateway;
using Discord.Commands;

namespace MusicBot
{
    [Command("skip")]
    public class SkipCommand : CommandBase
    {
        public override void Execute()
        {
            if (Program.CanModifyList(Client, Message))
            {
                var list = Program.TrackLists[Message.Guild.Id];

                var currentSong = list.Tracks[0];
                list.Tracks.RemoveAt(0);
                currentSong.CancellationTokenSource.Cancel();

                Message.Channel.SendMessage("Skipped the current song.");
            }
        }
    }
}

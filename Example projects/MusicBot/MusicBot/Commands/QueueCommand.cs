using Discord;
using Discord.Commands;

namespace MusicBot
{
    [Command("queue", "Shows the queue")]
    public class QueueCommand : CommandBase
    {
        public override void Execute()
        {
            if (Message.Guild != null)
            {
                if (Program.Players.TryGetValue(Message.Guild.Id, out MusicPlayer player) && player.Tracks.Count > 0)
                {
                    EmbedMaker embed = new EmbedMaker()
                    {
                        Title = "Current queue",
                        Description = $"Current track: {player.Tracks[0].Video.Title}\n{player.Tracks.Count - 1} song(s) are queued",
                        Color = Program.EmbedColor
                    };

                    for (int i = 1; i < player.Tracks.Count; i++)
                        embed.AddField(player.Tracks[i].Video.Title, player.Tracks[i].Video.Author);

                    Message.Channel.SendMessage("", false, embed);
                }
                else
                    Message.Channel.SendMessage("The queue is empty");
            }
        }
    }
}

using Discord;
using Discord.Commands;
using Discord.Gateway;

namespace MusicBot
{
    [Command("pause", "Pauses/unpauses the song")]
    public class PauseCommand : ICommand
    {
        public void Execute(DiscordSocketClient client, DiscordMessage message)
        {
            if (!Program.Sessions.ContainsKey(message.Guild))
                message.Channel.SendMessage("Not connected to a voice channel.");
            else
            {
                var session = Program.Sessions[message.Guild];

                session.Paused = !session.Paused;

                message.Channel.SendMessage(session.Paused ? "Paused." : "Unpaused.");
            }
        }
    }
}

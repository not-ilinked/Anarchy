using Discord;
using Discord.Commands;
using Discord.Gateway;

namespace MusicBot
{
    [Command("leave", "Makes the bot leave the current channel")]
    public class LeaveCommand : ICommand
    {
        public void Execute(DiscordSocketClient client, DiscordMessage message)
        {
            if (!Program.Sessions.ContainsKey(message.Guild))
                message.Channel.SendMessage("Bot is not connected to a voice channel.");
            else
            {
                Program.Sessions[message.Guild].Disconnect();

                message.Channel.SendMessage("Left the channel.");
            }
        }
    }
}

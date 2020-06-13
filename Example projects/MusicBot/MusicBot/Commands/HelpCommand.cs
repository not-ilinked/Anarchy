using Discord;
using Discord.Commands;
using Discord.Gateway;

namespace MusicBot
{
    [Command("help", "Shows a help menu")]
    public class HelpCommand : ICommand
    {
        public void Execute(DiscordSocketClient client, DiscordMessage message)
        {
            EmbedMaker embed = new EmbedMaker();
            embed.Title = "Music Bot Commands";
            embed.Color = Program.EmbedColor;
            embed.Footer.Text = Program.EmbedFooter.Text;
            embed.Footer.IconUrl = Program.EmbedFooter.IconUrl;
            
            foreach (var command in client.CommandHandler.Commands)
                embed.AddField(client.CommandHandler.Prefix + command.ToString(), command.Description);

            message.Channel.SendMessage("", false, embed);
        }
    }
}

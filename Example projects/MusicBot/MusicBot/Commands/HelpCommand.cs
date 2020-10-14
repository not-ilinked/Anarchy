using Discord;
using Discord.Commands;
using System.Text;

namespace MusicBot
{
    [Command("help", "Shows this help menu")]
    public class HelpCommand : CommandBase
    {
        public override void Execute()
        {
            EmbedMaker embed = new EmbedMaker()
            {
                Title = "Commands",
                Description = "A list of commands. Duh...",
                Color = Program.EmbedColor
            };

            foreach (var cmd in Client.CommandHandler.Commands.Values)
            {
                StringBuilder args = new StringBuilder();

                foreach (var arg in cmd.Parameters)
                    args.Append($" [{arg.Name}]");

                embed.AddField($"{Client.CommandHandler.Prefix + cmd.Name}{args}", cmd.Description);
            }

            Message.Channel.SendMessage("", false, embed);
        }
    }
}

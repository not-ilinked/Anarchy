using Discord;
using Discord.Commands;
using System.Drawing;
using System.Text;

namespace CommandListener
{
    [Command("help")]
    public class HelpCommand : CommandBase
    {
        public override void Execute()
        {
            EmbedMaker embed = new EmbedMaker()
            {
                Title = "Help",
                Description = "A list of commands",
                Color = Color.FromArgb(128, 0, 128),
                Footer = new EmbedFooter() { Text = "Powered by Anarchy", IconUrl = "https://anarchyteam.dev/favicon.ico" }
            };

            foreach (var cmd in Client.CommandHandler.Commands.Values)
            {
                StringBuilder args = new StringBuilder();

                foreach (var arg in cmd.Parameters)
                {
                    if (arg.Optional)
                        args.Append($" <{arg.Name}>");
                    else
                        args.Append($" [{arg.Name}]");
                }

                embed.AddField(Client.CommandHandler.Prefix + cmd.Name, $"{cmd.Name}{args}");
            }

            Message.Channel.SendMessage("", false, embed);
        }
    }
}

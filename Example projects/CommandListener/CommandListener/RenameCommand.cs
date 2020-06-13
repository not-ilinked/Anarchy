using Discord;
using Discord.Commands;
using Discord.Gateway;

namespace CommandListener
{
    // This command renames a text channel in the server

    [Command("rename")]
    public class RenameCommand : ICommand
    {
        [Parameter("channel")]
        public MinimalTextChannel Channel { get; private set; }


        [Parameter("new name")]
        public string NewName { get; private set; }


        // This will be executed whenever the command ;rename is sent through a channel
        public void Execute(DiscordSocketClient client, DiscordMessage message)
        {
            if (message.Guild != null)
            {
                if (message.Author.Member.HasPermission(DiscordPermission.ManageChannels))
                {
                    try
                    {
                        Channel.Modify(NewName);

                        message.Channel.SendMessage($"Success! channel has been renamed to \"{NewName}\"");
                    }
                    catch (DiscordHttpException ex)
                    {
                        if (ex.Code == DiscordError.UnknownChannel)
                            message.Channel.SendMessage("Unknown channel.");
                        else
                            message.Channel.SendMessage("An error has occured.");
                    }
                }
                else
                    message.Channel.SendMessage("Missing permissions.");
            }
        }
    }
}

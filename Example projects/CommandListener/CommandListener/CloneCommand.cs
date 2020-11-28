using Discord;
using Discord.Commands;
using System;

namespace CommandListener
{
    // This command renames a text channel in the server

    [Command("rename")]
    public class CloneCommand : CommandBase
    {
        // You'll have to use MinimalChannel or MinimalTextChannel if you have caching disabled.
        // We can assume that it'll be a text channel because
        // a. You can't mention non-text channels
        // b. Channels can't be mentioned in DM's
        [Parameter("channel")]
        public TextChannel Channel { get; private set; }


        // This can be multiple words since it's the last argument
        [Parameter("new name")]
        public string NewName { get; private set; }


        // This will be executed whenever the command ;rename is sent through a channel
        public override void Execute()
        {
            if (Message.Guild != null)
            {
                if (Message.Author.Member.GetPermissions().HasFlag(DiscordPermission.ManageChannels))
                {
                    try
                    {
                        Channel.Modify(new GuildChannelProperties() { Name = NewName });

                        Message.Channel.SendMessage($"Success! channel has been renamed to \"{NewName}\"");
                    }
                    catch (DiscordHttpException ex)
                    {
                        if (ex.Code == DiscordError.UnknownChannel)
                            Message.Channel.SendMessage("Unknown channel.");
                        else
                            Message.Channel.SendMessage("An error has occured.");
                    }
                }
                else
                    Message.Channel.SendMessage("Missing permissions.");
            }
        }

        public override void HandleError(string parameterName, string providedValue, Exception exception)
        {
            if (providedValue == null)
                Message.Channel.SendMessage("Please provide a value for " + parameterName);
            else
                Message.Channel.SendMessage("Invalid value on " + parameterName);
        }
    }
}

using Discord;
using Discord.Commands;
using System;

namespace CommandListener
{
    [Command("echo")]
    public class EchoCommand : CommandBase
    {
        [Parameter("message")]
        public string Echo { get; private set; }

        public override void Execute()
        {
            Message.Channel.SendMessage(Echo);
        }

        public override void HandleError(string parameterName, string providedValue, Exception exception)
        {
            if (providedValue == null)
                Message.Channel.SendMessage($"Please provide a value for '{parameterName}'");
        }
    }
}

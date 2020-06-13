using System;

namespace Discord.Commands
{
    public class MissingParameterEventArgs : EventArgs
    {
        public DiscordCommand Command { get; private set; }
        public ParameterAttribute Parameter { get; private set; }

        public MissingParameterEventArgs(DiscordCommand command, ParameterAttribute param)
        {
            Command = command;
            Parameter = param;
        }
    }
}

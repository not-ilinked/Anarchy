using Discord.Gateway;
using System;
using System.Collections.Generic;

namespace Discord.Commands
{
    public abstract class CommandBase
    {
        public DiscordSocketClient Client { get; private set; }
        public DiscordMessage Message { get; private set; }

        internal void Prepare(DiscordSocketClient client, DiscordMessage message)
        {
            Client = client;
            Message = message;
        }

        public abstract void Execute();
        public virtual void HandleError(string parameterName, string providedValue, CommandError error) { }
    }
}

using System;

namespace Discord.Gateway
{
    public class DiscordInteractionEventArgs : EventArgs
    {
        public DiscordInteraction Interaction { get; private set; }

        public DiscordInteractionEventArgs(DiscordInteraction interaction)
        {
            Interaction = interaction;
        }
    }
}

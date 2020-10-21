using System;

namespace Discord.Gateway
{
    public class NitroBoostEventArgs : EventArgs
    {
        public DiscordBoostSlot Slot { get; private set; }


        internal NitroBoostEventArgs(DiscordBoostSlot boost)
        {
            Slot = boost;
        }
    }
}

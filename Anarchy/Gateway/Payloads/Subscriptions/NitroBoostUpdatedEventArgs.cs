using System;

namespace Discord.Gateway
{
    public class NitroBoostUpdatedEventArgs : EventArgs
    {
        public DiscordNitroBoost Boost { get; private set; }


        internal NitroBoostUpdatedEventArgs(DiscordNitroBoost boost)
        {
            Boost = boost;
        }
    }
}

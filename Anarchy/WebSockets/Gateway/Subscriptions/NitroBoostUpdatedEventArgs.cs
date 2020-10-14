using System;

namespace Discord.Gateway
{
    public class NitroBoostUpdatedEventArgs : EventArgs
    {
        public DiscordGuildBoost Boost { get; private set; }


        internal NitroBoostUpdatedEventArgs(DiscordGuildBoost boost)
        {
            Boost = boost;
        }
    }
}

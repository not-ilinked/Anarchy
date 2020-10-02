using System;

namespace Discord.Gateway
{
    public class GuildUnavailableEventArgs : EventArgs
    {
        public UnavailableGuild Guild { get; private set; }

        public GuildUnavailableEventArgs(UnavailableGuild guild)
        {
            Guild = guild;
        }
    }
}

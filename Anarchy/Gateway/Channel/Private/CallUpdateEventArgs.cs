using System;

namespace Discord.Gateway
{
    public class CallUpdateEventArgs : EventArgs
    {
        public DiscordCall Call { get; private set; }

        public CallUpdateEventArgs(DiscordCall call)
        {
            Call = call;
        }
    }
}

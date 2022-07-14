using System;

namespace Discord.Gateway
{
    public class ThreadEventArgs : EventArgs
    {
        public DiscordThread Thread { get; }

        public ThreadEventArgs(DiscordThread thread)
        {
            Thread = thread;
        }
    }
}

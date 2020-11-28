using System;

namespace Discord.Media
{
    public class ChannelChangedEventArgs : EventArgs
    {
        public ulong OldChannelId { get; private set; }
        public ulong NewChannelId { get; private set; }

        public ChannelChangedEventArgs(ulong old, ulong @new)
        {
            OldChannelId = old;
            NewChannelId = @new;
        }
    }
}

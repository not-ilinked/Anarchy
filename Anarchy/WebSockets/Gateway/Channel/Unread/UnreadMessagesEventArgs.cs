using System;
using System.Collections.Generic;

namespace Discord.Gateway
{
    public class UnreadMessagesEventArgs : EventArgs
    {
        public MinimalGuild Guild { get; private set; }
        public IReadOnlyList<ChannelUnreadMessages> Channels { get; private set; }

        public UnreadMessagesEventArgs(GuildUnreadMessages unread)
        {
            Guild = unread.Guild;
            Channels = unread.Channels;
        }
    }
}

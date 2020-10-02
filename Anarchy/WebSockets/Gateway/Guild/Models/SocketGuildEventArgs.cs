using System;

namespace Discord.Gateway
{
    public class SocketGuildEventArgs : EventArgs
    {
        public SocketGuild Guild { get; private set; }

        public bool Lurking { get; private set; }

        public SocketGuildEventArgs(SocketGuild guild, bool lurking)
        {
            Guild = guild;
            Lurking = lurking;
        }
    }
}

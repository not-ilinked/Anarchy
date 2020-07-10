using System;

namespace Discord.Gateway
{
    public class GuildMemberEventArgs : EventArgs
    {
        public GuildMember Member { get; private set; }

        internal GuildMemberEventArgs(GuildMember member)
        {
            Member = member;
        }
    }
}

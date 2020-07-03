using System;

namespace Discord.Gateway
{
    public class MemberRemovedEventArgs : EventArgs
    {
        public PartialGuildMember Member { get; private set; }

        public MemberRemovedEventArgs(PartialGuildMember member)
        {
            Member = member;
        }
    }
}

using Discord.Gateway;
using System;
using System.Collections.Generic;

namespace Discord
{
    public class GuildMembersEventArgs : EventArgs
    {
        public ulong GuildId { get; private set; }
        public IReadOnlyList<GuildMember> Members { get; private set; }
        
        public int Index { get; private set; }
        public int Total { get; private set; }
        
        internal GuildMembersEventArgs(GuildMemberList members)
        {
            Index = members.ChunkIndex;
            Total = members.ChunkCount;
            GuildId = members.GuildId;
            Members = members.Members;
        }
    }
}

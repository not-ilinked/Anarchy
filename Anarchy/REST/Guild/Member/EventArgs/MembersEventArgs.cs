using Discord.Gateway;
using System;
using System.Collections.Generic;

namespace Discord
{
    public class GuildMembersEventArgs : EventArgs
    {
        public ulong GuildId { get; private set; }
        public IReadOnlyList<GuildMember> Members { get; private set; }
        public bool? Sync { get; private set; }
        
        public int? Index { get; private set; }
        public int Total { get; private set; }
        
        internal GuildMembersEventArgs(GuildMemberList members)
        {
            Index = members.ChunkIndex;
            Total = members.ChunkCount;
            GuildId = members.GuildId;
            Members = members.Members;
        }

        internal GuildMembersEventArgs(GatewayUserMemberQueryResponse memberPayload)
        {
            List<GuildMember> members = new List<GuildMember>();

            foreach (var range in memberPayload.Ranges)
            {
                if (range.Opcode == "SYNC")
                {
                    Sync = true;

                    foreach (var member in range.Items)
                    {
                        if (member.Member != null)
                            members.Add(member.Member);
                    }
                }
            }

            if (!Sync.HasValue)
                Sync = false;

            GuildId = memberPayload.GuildId;
            Members = members;
            Total = memberPayload.MemberCount;
        }
    }
}

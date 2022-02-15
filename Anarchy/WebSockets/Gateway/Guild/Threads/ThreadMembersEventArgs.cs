using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord.Gateway
{
    public class ThreadMembersEventArgs : Controllable
    {
        public ThreadMembersEventArgs()
        {
            OnClientUpdated += (s, e) => AddedMembers.SetClientsInList(Client);
        }

        [JsonProperty("id")]
        public ulong Id { get; private set; }

        [JsonProperty("guild_id")]
        private ulong _guildId;
        public MinimalGuild Guild => new MinimalGuild(_guildId).SetClient(Client);

        [JsonProperty("member_count")]
        public int MemberCount { get; private set; }

        [JsonProperty("added_members")]
        public IReadOnlyList<DiscordThreadMember> AddedMembers { get; private set; }

        [JsonProperty("removed_member_ids")]
        public IReadOnlyList<ulong> RemovedMembers { get; private set; }
    }
}

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class ThreadMembersEventArgs : Controllable
    {
        public ThreadMembersEventArgs()
        {
            OnClientUpdated += (s, e) => AddedMembers.SetClientsInList(Client);
        }

        [JsonPropertyName("id")]
        public ulong Id { get; private set; }

        [JsonPropertyName("guild_id")]
        private readonly ulong _guildId;
        public MinimalGuild Guild => new MinimalGuild(_guildId).SetClient(Client);

        [JsonPropertyName("member_count")]
        public int MemberCount { get; private set; }

        [JsonPropertyName("added_members")]
        public IReadOnlyList<DiscordThreadMember> AddedMembers { get; private set; }

        [JsonPropertyName("removed_member_ids")]
        public IReadOnlyList<ulong> RemovedMembers { get; private set; }
    }
}

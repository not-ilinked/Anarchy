using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class GuildMemberList : Controllable
    {
        public GuildMemberList()
        {
            OnClientUpdated += (sender, e) => Members.SetClientsInList(Client);
        }

        private ulong _guildId;
        [JsonPropertyName("guild_id")]
        public ulong GuildId
        {
            get { return _guildId; }
            set
            {
                _guildId = value;
                foreach (var member in Members)
                    member.GuildId = _guildId;
            }
        }

        [JsonPropertyName("members")]
        public IReadOnlyList<GuildMember> Members { get; private set; }

        [JsonPropertyName("chunk_index")]
        public int ChunkIndex { get; private set; }

        [JsonPropertyName("chunk_count")]
        public int ChunkCount { get; private set; }
    }
}
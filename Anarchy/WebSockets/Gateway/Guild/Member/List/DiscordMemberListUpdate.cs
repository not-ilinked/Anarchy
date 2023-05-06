using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class DiscordMemberListUpdate : Controllable
    {
        public DiscordMemberListUpdate()
        {
            OnClientUpdated += (s, e) => Operations.SetClientsInList(Client);
        }

        [JsonPropertyName("ops")]
        public IReadOnlyList<MemberListUpdateOperation> Operations { get; private set; }

        [JsonPropertyName("online_count")]
        public uint OnlineMembers { get; private set; }

        [JsonPropertyName("member_count")]
        public uint Members { get; private set; }

        [JsonPropertyName("guild_id")]
        private readonly ulong _guildId;

        public MinimalGuild Guild => new MinimalGuild(_guildId).SetClient(Client);

        [JsonPropertyName("groups")]
        public IReadOnlyList<MemberListGroup> Groups { get; private set; }
    }
}

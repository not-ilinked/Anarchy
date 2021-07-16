using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Gateway
{
    public class DiscordMemberListUpdate : Controllable
    {
        public DiscordMemberListUpdate()
        {
            OnClientUpdated += (s, e) => Operations.SetClientsInList(Client);
        }

        [JsonProperty("ops")]
        public IReadOnlyList<MemberListUpdateOperation> Operations { get; private set; }

        [JsonProperty("online_count")]
        public uint OnlineMembers { get; private set; }

        [JsonProperty("member_count")]
        public uint Members { get; private set; }

        [JsonProperty("guild_id")]
        private readonly ulong _guildId;

        public MinimalGuild Guild => new MinimalGuild(_guildId).SetClient(Client);

        [JsonProperty("groups")]
        public IReadOnlyList<MemberListGroup> Groups { get; private set; }
    }
}

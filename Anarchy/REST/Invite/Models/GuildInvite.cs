using Newtonsoft.Json;

namespace Discord
{
    public class GuildInvite : DiscordInvite
    {
        [JsonProperty("temporary")]
        public bool Temporary { get; private set; }


        [JsonProperty("uses")]
        public uint Uses { get; private set; }


        [JsonProperty("max_uses")]
        public uint MaxUses { get; private set; }


        [JsonProperty("approximate_presence_count")]
        public uint OnlineMembers { get; private set; }


        [JsonProperty("approximate_member_count")]
        public uint TotalMembers { get; private set; }
    }
}

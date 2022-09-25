using Newtonsoft.Json;

namespace Discord
{
    public class GuildInvite : DiscordInvite
    {
        public GuildInvite()
        {
            OnClientUpdated += (s, e) =>
            {
                if (_guild != null)
                {
                    ((GuildChannel) Channel).GuildId = _guild.Id;
                    _guild.SetClient(Client);
                }
            };
        }

        public InviteGuild Guild => _guild;

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

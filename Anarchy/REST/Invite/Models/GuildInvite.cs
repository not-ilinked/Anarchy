

using System.Text.Json.Serialization;

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

        [JsonPropertyName("temporary")]
        public bool Temporary { get; private set; }

        [JsonPropertyName("uses")]
        public uint Uses { get; private set; }

        [JsonPropertyName("max_uses")]
        public uint MaxUses { get; private set; }

        [JsonPropertyName("approximate_presence_count")]
        public uint OnlineMembers { get; private set; }

        [JsonPropertyName("approximate_member_count")]
        public uint TotalMembers { get; private set; }
    }
}

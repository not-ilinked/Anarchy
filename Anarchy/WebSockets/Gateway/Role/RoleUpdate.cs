

using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    internal class RoleUpdate : Controllable
    {
        public RoleUpdate()
        {
            OnClientUpdated += (sender, e) => Role.SetClient(Client);
        }

        private ulong _guildId;
        [JsonPropertyName("guild_id")]
        public ulong GuildId
        {
            get { return _guildId; }
            set { Role.GuildId = _guildId = value; }
        }

        [JsonPropertyName("role")]
        public DiscordRole Role { get; private set; }

        public override string ToString()
        {
            return Role.ToString();
        }
    }
}

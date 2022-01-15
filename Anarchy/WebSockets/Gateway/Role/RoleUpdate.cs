using Newtonsoft.Json;

namespace Discord.Gateway
{
    internal class RoleUpdate : Controllable
    {
        public RoleUpdate()
        {
            OnClientUpdated += (sender, e) => Role.SetClient(Client);
        }

        private ulong _guildId;
        [JsonProperty("guild_id")]
        public ulong GuildId
        {
            get => _guildId;
            set => Role.GuildId = _guildId = value;
        }


        [JsonProperty("role")]
        public DiscordRole Role { get; private set; }


        public override string ToString()
        {
            return Role.ToString();
        }
    }
}



using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class BanUpdateEventArgs : Controllable
    {
        public BanUpdateEventArgs()
        {
            OnClientUpdated += (sender, e) =>
            {
                User.SetClient(Client);
            };
        }

        [JsonPropertyName("guild_id")]
        private readonly ulong _guildId;

        public MinimalGuild Guild
        {
            get
            {
                return new MinimalGuild(_guildId).SetClient(Client);
            }
        }

        [JsonPropertyName("user")]
        public DiscordUser User { get; private set; }

        public override string ToString()
        {
            return User.ToString();
        }
    }
}

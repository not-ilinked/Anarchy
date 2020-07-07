using Newtonsoft.Json;

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

        [JsonProperty("guild_id")]
        private readonly ulong _guildId;

        public MinimalGuild Guild
        {
            get
            {
                return new MinimalGuild(_guildId).SetClient(Client);
            }
        }


        [JsonProperty("user")]
        public DiscordUser User { get; private set; }


        public override string ToString()
        {
            return User.ToString();
        }
    }
}

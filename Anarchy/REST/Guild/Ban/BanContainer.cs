using Newtonsoft.Json;

namespace Discord.Gateway
{
    internal class BanContainer : Controllable
    {
        public BanContainer()
        {
            OnClientUpdated += (sender, e) => User.SetClient(Client);
        }


        [JsonProperty("guild_id")]
        public ulong GuildId { get; private set; }


        [JsonProperty("user")]
        public DiscordUser User { get; private set; }


        public override string ToString()
        {
            return User.ToString();
        }
    }
}

using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class LoginGuild : MinimalGuild
    {
        [JsonProperty("unavailable")]
        public bool Unavailable { get; private set; }

        /// <summary>
        /// Gets the full guild.
        /// Please only use this method if the account type is User
        /// </summary>
        public SocketGuild ToSocketGuild()
        {
            return Json.ToObjectEx<SocketGuild>().SetClient(Client);
        }
    }
}

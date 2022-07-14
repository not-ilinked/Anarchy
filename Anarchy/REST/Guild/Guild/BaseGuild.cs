using Newtonsoft.Json;

namespace Discord
{
    public class BaseGuild : MinimalGuild
    {
        [JsonProperty("name")]
        public string Name { get; private set; }


        [JsonProperty("icon")]
        private string _iconHash;


        public DiscordCDNImage Icon
        {
            get
            {
                if (_iconHash == null)
                    return null;
                else
                    return new DiscordCDNImage(CDNEndpoints.GuildIcon, Id, _iconHash);
            }
        }


        protected void Update(BaseGuild guild)
        {
            Name = guild.Name;
            _iconHash = guild._iconHash;
        }


        public override string ToString()
        {
            return Name;
        }
    }
}

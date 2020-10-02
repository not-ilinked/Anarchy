using Newtonsoft.Json;

namespace Discord
{
    public class BaseGuild : MinimalGuild
    {
        [JsonProperty("name")]
        public string Name { get; protected set; }


        [JsonProperty("icon")]
        protected string _iconHash;


        public DiscordCDNMedia Icon
        {
            get 
            {
                if (_iconHash == null)
                    return null;
                else
                    return new DiscordCDNMedia(CDNEndpoints.GuildIcon, Id, _iconHash); 
            }
        }


        public override string ToString()
        {
            return Name;
        }
    }
}

using Newtonsoft.Json;

namespace Discord
{
    public abstract class BaseGuild : MinimalGuild
    {
        [JsonProperty("name")]
        public string Name { get; protected set; }


        [JsonProperty("icon")]
        protected string _iconId;


        public DiscordGuildIcon Icon
        {
            get
            {
                return new DiscordGuildIcon(Id, _iconId);
            }
        }


        public override string ToString()
        {
            return Name;
        }
    }
}

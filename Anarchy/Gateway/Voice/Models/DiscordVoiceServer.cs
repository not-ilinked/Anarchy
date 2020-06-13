using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class DiscordVoiceServer
    {
        [JsonProperty("token")]
        public string Token { get; private set; }


        [JsonProperty("guild_id")]
        private readonly ulong? _guildId;


        public MinimalGuild Guild
        {
            get
            {
                if (_guildId.HasValue)
                    return new MinimalGuild(_guildId.Value);
                else
                    return null;
            }
        }


        [JsonProperty("endpoint")]
        public string Server { get; private set; }
    }
}
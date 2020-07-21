using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class DiscordMediaServer : Controllable
    {
        [JsonProperty("token")]
        public string Token { get; private set; }


        [JsonProperty("guild_id")]
        internal ulong? GuildId { get; set; }


        public MinimalGuild Guild
        {
            get
            {
                if (GuildId.HasValue)
                    return new MinimalGuild(GuildId.Value).SetClient(Client);
                else
                    return null;
            }
        }


        [JsonProperty("endpoint")]
        public string Server { get; private set; }

        
        [JsonProperty("stream_key")]
        internal string StreamKey { get; private set; }
    }
}
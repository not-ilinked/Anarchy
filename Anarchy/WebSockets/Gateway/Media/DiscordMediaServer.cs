

using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class DiscordMediaServer : Controllable
    {
        [JsonPropertyName("token")]
        public string Token { get; private set; }

        [JsonPropertyName("guild_id")]
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

        private string _endpoint;
        [JsonPropertyName("endpoint")]
        public string Endpoint
        {
            get { return _endpoint; }
            set
            {
                if (value != null)
                    _endpoint = value.Split(':')[0];
            }
        }

        [JsonPropertyName("stream_key")]
        internal string StreamKey { get; private set; }
    }
}
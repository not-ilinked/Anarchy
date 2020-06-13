using Newtonsoft.Json;

namespace Discord.Voice
{
    public class DiscordVoiceIdentify
    {
        [JsonProperty("server_id")]
        public ulong GuildId { get; set; }


        [JsonProperty("user_id")]
        public ulong UserId { get; set; }


        [JsonProperty("session_id")]
        public string SessionId { get; set; }


        [JsonProperty("token")]
        public string Token { get; set; }


        [JsonProperty("video")]
        public bool Video { get; set; }
    }
}

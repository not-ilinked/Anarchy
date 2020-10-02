using Newtonsoft.Json;

namespace Discord.Media
{
    internal class DiscordMediaIdentify
    {
        [JsonProperty("server_id")]
        public ulong ServerId { get; set; }


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

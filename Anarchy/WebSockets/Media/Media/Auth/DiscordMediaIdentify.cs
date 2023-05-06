

using System.Text.Json.Serialization;

namespace Discord.Media
{
    internal class DiscordMediaIdentify
    {
        [JsonPropertyName("server_id")]
        public ulong ServerId { get; set; }

        [JsonPropertyName("user_id")]
        public ulong UserId { get; set; }

        [JsonPropertyName("session_id")]
        public string SessionId { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("video")]
        public bool Video { get; set; }
    }
}

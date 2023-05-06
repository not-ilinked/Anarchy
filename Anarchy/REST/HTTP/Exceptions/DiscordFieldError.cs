

using System.Text.Json.Serialization;

namespace Discord
{
    public class DiscordFieldError
    {
        [JsonPropertyName("code")]
        public string Code { get; private set; }

        [JsonPropertyName("message")]
        public string Message { get; private set; }
    }
}

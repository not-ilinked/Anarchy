
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord
{
    public class DiscordHttpError
    {
        [JsonPropertyName("code")]
        public DiscordError Code { get; private set; }

        [JsonPropertyName("errors")]
        public JsonElement Fields { get; private set; }

        [JsonPropertyName("message")]
        public string Message { get; private set; }

        public DiscordHttpError() { }

        public DiscordHttpError(DiscordError code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}

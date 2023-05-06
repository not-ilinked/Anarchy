

using System.Text.Json.Serialization;

namespace Discord.Media
{
    internal class SSRCUpdate : DiscordSSRC
    {
        [JsonPropertyName("user_id")]
        public ulong UserId { get; private set; }
    }
}

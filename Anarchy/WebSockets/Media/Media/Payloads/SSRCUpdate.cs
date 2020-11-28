using Newtonsoft.Json;

namespace Discord.Media
{
    internal class SSRCUpdate : DiscordSSRC
    {
        [JsonProperty("user_id")]
        public ulong UserId { get; private set; }
    }
}

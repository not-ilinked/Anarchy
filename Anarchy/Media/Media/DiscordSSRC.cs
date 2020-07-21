using Newtonsoft.Json;

namespace Discord.Media
{
    internal class DiscordSSRC : SSRCUpdate
    {
        [JsonProperty("user_id")]
        public ulong UserId { get; private set; }
    }
}

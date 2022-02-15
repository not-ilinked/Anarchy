using Newtonsoft.Json;

namespace Discord
{
    public class DiscordProfileUser : DiscordUser
    {
        [JsonProperty("bio")]
        public string Biography { get; private set; }

        [JsonProperty("banner")]
        private readonly string _bannerHash;
        public DiscordCDNImage Banner => _bannerHash == null ? null : new DiscordCDNImage(CDNEndpoints.Banner, Id, _bannerHash);
    }
}

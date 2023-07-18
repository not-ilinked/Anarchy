using Newtonsoft.Json;

namespace Discord
{
    public class DiscordProfileUser : DiscordUser
    {
        [JsonProperty("global_name")]
        public string GlobalName { get; private set; }

        [JsonProperty("bio")]
        public string Biography { get; private set; }

        [JsonProperty("banner_color")]
        public int? BannerColor { get; private set; }

        [JsonProperty("accent_color")]
        public int? AccentColor { get; private set; }

        [JsonProperty("avatar_decoration")]
        public int? AvatarDecoration { get; private set; } // unsure of the type here assuming int

        [JsonProperty("banner")]
        private readonly string _bannerHash;
        public DiscordCDNImage Banner => _bannerHash == null ? null : new DiscordCDNImage(CDNEndpoints.Banner, Id, _bannerHash);
    }
}

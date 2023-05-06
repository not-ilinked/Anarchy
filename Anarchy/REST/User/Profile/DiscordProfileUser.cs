

using System.Text.Json.Serialization;
namespace Discord
{
    public class DiscordProfileUser : DiscordUser
    {
        [JsonPropertyName("bio")]
        public string Biography { get; private set; }

        [JsonPropertyName("banner")]
        private readonly string _bannerHash;
        public DiscordCDNImage Banner => _bannerHash == null ? null : new DiscordCDNImage(CDNEndpoints.Banner, Id, _bannerHash);
    }
}

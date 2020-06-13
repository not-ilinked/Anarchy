using System.Collections.Generic;

namespace Discord
{
    public class DiscordDiscoverySplashCDNImage : DiscordHashedCDNImage
    {
        protected override string BaseEndpoint { get; set; } = "discovery-splashes";
        protected override List<DiscordCDNImageFormat> SupportedFormats { get; set; } = new List<DiscordCDNImageFormat>()
        {
            DiscordCDNImageFormat.PNG,
            DiscordCDNImageFormat.JPG,
            DiscordCDNImageFormat.WebP
        };


        public DiscordDiscoverySplashCDNImage(ulong id, string hash) : base(id, hash)
        { }
    }
}

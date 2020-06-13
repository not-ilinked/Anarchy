using System.Collections.Generic;

namespace Discord
{
    public class DiscordGuildBannerCDNImage : DiscordHashedCDNImage
    {
        protected override string BaseEndpoint { get; set; } = "banners";
        protected override List<DiscordCDNImageFormat> SupportedFormats { get; set; } = new List<DiscordCDNImageFormat>()
        {
            DiscordCDNImageFormat.PNG,
            DiscordCDNImageFormat.JPG,
            DiscordCDNImageFormat.WebP
        };


        public DiscordGuildBannerCDNImage(ulong id, string hash) : base(id, hash)
        { }
    }
}

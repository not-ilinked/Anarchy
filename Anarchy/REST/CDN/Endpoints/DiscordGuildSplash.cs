using System.Collections.Generic;

namespace Discord
{
    public class DiscordGuildSplash : DiscordHashedCDNImage
    {
        protected override string BaseEndpoint { get; set; } = "splashes";
        protected override List<DiscordCDNImageFormat> SupportedFormats { get; set; } = new List<DiscordCDNImageFormat>()
        {
            DiscordCDNImageFormat.PNG,
            DiscordCDNImageFormat.JPG,
            DiscordCDNImageFormat.WebP
        };

        public DiscordGuildSplash(ulong id, string hash) : base(id, hash)
        { }
    }
}

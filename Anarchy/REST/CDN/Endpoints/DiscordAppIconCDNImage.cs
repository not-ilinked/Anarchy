using System.Collections.Generic;

namespace Discord
{
    public class DiscordAppIconCDNImage : DiscordHashedCDNImage
    {
        protected override string BaseEndpoint { get; set; } = "app-icons";
        protected override List<DiscordCDNImageFormat> SupportedFormats { get; set; } = new List<DiscordCDNImageFormat>()
        {
            DiscordCDNImageFormat.PNG,
            DiscordCDNImageFormat.JPG,
            DiscordCDNImageFormat.WebP
        };


        public DiscordAppIconCDNImage(ulong id, string hash) : base(id, hash)
        { }
    }
}

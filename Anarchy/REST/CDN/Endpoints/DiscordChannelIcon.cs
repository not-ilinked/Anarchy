using System.Collections.Generic;

namespace Discord
{
    public class DiscordChannelIcon : DiscordHashedCDNImage
    {
        protected override string BaseEndpoint { get; set; } = "channel-icons";
        protected override List<DiscordCDNImageFormat> SupportedFormats { get; set; } = new List<DiscordCDNImageFormat>()
        {
            DiscordCDNImageFormat.PNG,
            DiscordCDNImageFormat.JPG,
            DiscordCDNImageFormat.WebP
        };

        public DiscordChannelIcon(ulong id, string hash) : base(id, hash)
        { }
    }
}

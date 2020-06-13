using System.Collections.Generic;

namespace Discord
{
    public class DiscordTeamIconCDNImage : DiscordHashedCDNImage
    {
        protected override string BaseEndpoint { get; set; } = "team-icons";
        protected override List<DiscordCDNImageFormat> SupportedFormats { get; set; } = new List<DiscordCDNImageFormat>()
        {
            DiscordCDNImageFormat.PNG,
            DiscordCDNImageFormat.JPG,
            DiscordCDNImageFormat.WebP
        };


        public DiscordTeamIconCDNImage(ulong id, string hash) : base(id, hash)
        { }
    }
}

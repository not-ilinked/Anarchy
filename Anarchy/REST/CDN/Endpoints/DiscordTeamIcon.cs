using System.Collections.Generic;

namespace Discord
{
    public class DiscordTeamIcon : DiscordHashedCDNImage
    {
        protected override string BaseEndpoint { get; set; } = "team-icons";
        protected override List<DiscordCDNImageFormat> SupportedFormats { get; set; } = new List<DiscordCDNImageFormat>()
        {
            DiscordCDNImageFormat.PNG,
            DiscordCDNImageFormat.JPG,
            DiscordCDNImageFormat.WebP
        };


        public DiscordTeamIcon(ulong id, string hash) : base(id, hash)
        { }
    }
}

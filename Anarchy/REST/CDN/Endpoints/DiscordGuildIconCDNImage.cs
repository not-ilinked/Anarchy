using System.Collections.Generic;

namespace Discord
{
    public class DiscordGuildIconCDNImage : DiscordHashedCDNImage
    {
        protected override string BaseEndpoint { get; set; } = "icons";
        protected override List<DiscordCDNImageFormat> SupportedFormats { get; set; }


        public DiscordGuildIconCDNImage(ulong id, string hash) : base(id, hash)
        { }
    }
}

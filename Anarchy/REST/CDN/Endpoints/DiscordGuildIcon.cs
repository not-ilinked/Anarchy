using System.Collections.Generic;

namespace Discord
{
    public class DiscordGuildIcon : DiscordHashedCDNImage
    {
        protected override string BaseEndpoint { get; set; } = "icons";
        protected override List<DiscordCDNImageFormat> SupportedFormats { get; set; }


        public DiscordGuildIcon(ulong id, string hash) : base(id, hash)
        { }
    }
}

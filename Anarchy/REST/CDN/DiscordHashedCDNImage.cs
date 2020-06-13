using System.Collections.Generic;

namespace Discord
{
    public class DiscordHashedCDNImage : DiscordCDNImage
    {
        protected override string BaseEndpoint { get; set; }
        protected override List<DiscordCDNImageFormat> SupportedFormats { get; set; }

        public string Hash { get; private set; }

        public DiscordHashedCDNImage(ulong id, string hash) : base(id, hash)
        {
            Hash = hash;
        }
    }
}

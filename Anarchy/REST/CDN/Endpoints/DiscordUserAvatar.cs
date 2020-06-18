using System.Collections.Generic;

namespace Discord
{
    public class DiscordUserAvatar : DiscordHashedCDNImage
    {
        protected override string BaseEndpoint { get; set; } = "avatars";
        protected override List<DiscordCDNImageFormat> SupportedFormats { get; set; }


        public DiscordUserAvatar(ulong id, string hash) : base(id, hash)
        { }
    }
}

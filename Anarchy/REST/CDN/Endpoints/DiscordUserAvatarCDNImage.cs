using System.Collections.Generic;

namespace Discord
{
    public class DiscordUserAvatarCDNImage : DiscordHashedCDNImage
    {
        protected override string BaseEndpoint { get; set; } = "avatars";
        protected override List<DiscordCDNImageFormat> SupportedFormats { get; set; }


        public DiscordUserAvatarCDNImage(ulong id, string hash) : base(id, hash)
        { }
    }
}

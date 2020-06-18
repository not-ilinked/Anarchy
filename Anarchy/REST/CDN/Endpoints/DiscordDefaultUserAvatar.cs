using System.Collections.Generic;

namespace Discord
{
    public class DiscordDefaultUserAvatar : DiscordCDNImage
    {
        protected override string BaseEndpoint { get; set; } = "embed/avatars";
        protected override List<DiscordCDNImageFormat> SupportedFormats { get; set; } = new List<DiscordCDNImageFormat>()
        {
            DiscordCDNImageFormat.PNG
        };


        public DiscordDefaultUserAvatar(int userDiscriminator) : base(userDiscriminator % 5)
        { }
    }
}

using System.Collections.Generic;

namespace Discord
{
    public class DiscordDefaultUserAvatarCDNImage : DiscordCDNImage
    {
        protected override string BaseEndpoint { get; set; } = "embed/avatars";
        protected override List<DiscordCDNImageFormat> SupportedFormats { get; set; } = new List<DiscordCDNImageFormat>()
        {
            DiscordCDNImageFormat.PNG
        };


        public DiscordDefaultUserAvatarCDNImage(int userDiscriminator) : base(userDiscriminator % 5)
        { }
    }
}

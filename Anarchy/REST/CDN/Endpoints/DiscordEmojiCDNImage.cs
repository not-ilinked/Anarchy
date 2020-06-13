using System.Collections.Generic;

namespace Discord
{
    public class DiscordEmojiCDNImage : DiscordCDNImage
    {
        protected override string BaseEndpoint { get; set; } = "emojis";
        protected override List<DiscordCDNImageFormat> SupportedFormats { get; set; } = new List<DiscordCDNImageFormat>()
        {
            DiscordCDNImageFormat.PNG,
            DiscordCDNImageFormat.GIF
        };


        public DiscordEmojiCDNImage(ulong id) : base(id)
        { }
    }
}

using System.Collections.Generic;

namespace Discord
{
    public class CDNEndpoint
    {
        public string Template { get; private set; }
        public IReadOnlyList<DiscordCDNImageFormat> AllowedFormats { get; private set; }

        public CDNEndpoint(string template, List<DiscordCDNImageFormat> allowedFormats)
        {
            Template = template;
            AllowedFormats = allowedFormats;
        }
    }
}

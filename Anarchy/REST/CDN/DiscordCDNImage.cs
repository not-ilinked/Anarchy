using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    public class DiscordCDNImage
    {
        private readonly HttpClient _client;
        public string Url { get; private set; }
        public object[] Particles { get; private set; }
        public IReadOnlyList<DiscordCDNImageFormat> AllowedFormats { get; private set; }

        public DiscordCDNImage(CDNEndpoint endpoint, params object[] assets)
        {
            _client = new HttpClient();
            Url = "https://cdn.discordapp.com/" +  string.Format(endpoint.Template, assets);
            Particles = assets;
            AllowedFormats = endpoint.AllowedFormats;
        }


        public DiscordImage Download(DiscordCDNImageFormat format = DiscordCDNImageFormat.Any)
        {
            if (format != DiscordCDNImageFormat.Any && !AllowedFormats.Contains(format))
                throw new NotSupportedException("Image format not supported. The supported formats for this endpoint are: " + string.Join(", ", AllowedFormats));

            string url = Url;

            if (format != DiscordCDNImageFormat.Any)
                url += "." + format.ToString().ToLower();

            return new DiscordImage((Bitmap)new ImageConverter().ConvertFrom(_client.GetByteArrayAsync(url).Result));
        }
    }
}

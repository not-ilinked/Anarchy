using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;

namespace Discord
{
    public abstract class DiscordCDNImage
    {
        protected abstract string BaseEndpoint { get; set; }
        protected abstract List<DiscordCDNImageFormat> SupportedFormats { get; set; }
        public string Url { get; protected set; }
        public ulong Id { get; private set; }

        protected DiscordCDNImage(params object[] information)
        {
            Id = (ulong)information[0];

            Url = $"https://cdn.discordapp.com/{BaseEndpoint}/{string.Join("/", information)}";
        }


        public DiscordImage Download(DiscordCDNImageFormat format = DiscordCDNImageFormat.Any)
        {
            if (format != DiscordCDNImageFormat.Any && SupportedFormats != null && !SupportedFormats.Contains(format))
                throw new NotSupportedException("Image format not supported. Supported formats for this endpoint: " + string.Join(", ", SupportedFormats));

            string extension = format == DiscordCDNImageFormat.Any ? "" : $".{format.ToString().ToLower()}";

            try
            {
                return new DiscordImage((Bitmap)new ImageConverter().ConvertFrom(new HttpClient().GetByteArrayAsync(Url + extension).Result));
            }
            catch
            {
                return null;
            }
        }
    }
}

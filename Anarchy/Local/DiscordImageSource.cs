using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;

namespace Discord
{
    public static class DiscordImageSource
    {
        public static async Task<DiscordImage> FromUrl(string url)
        {
            using var hc = new HttpClient();
            using var response = await hc.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return FromStream(
                await response.Content.ReadAsStreamAsync(),
                DiscordImageMediaType.ToImageFormat(response.Content.Headers.First(x => x.Key == "Content-Type").Value.First()));
        }

        public static DiscordImage FromFile(DiscordAttachmentFile file)
        {
            return FromBytes(file.Bytes, DiscordImageMediaType.ToImageFormat(file.MediaType));
        }

        public static DiscordImage FromStream(Stream stream, ImageFormat format)
        {
            return new DiscordImage(
                PlatformImage.FromStream(stream, format),
                format
            );
        }

        public static DiscordImage FromBytes(byte[] bytes, ImageFormat format)
        {
            using var stream = new MemoryStream(bytes);
            return FromStream(stream, format);
        }
    }
}

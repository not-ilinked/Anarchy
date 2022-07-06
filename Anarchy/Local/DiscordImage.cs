using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using Newtonsoft.Json;

namespace Discord
{
    class ImageJsonConverter : JsonConverter
    {
        public override bool CanRead => false;

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }

    [JsonConverter(typeof(ImageJsonConverter))]
    public class DiscordImage
    {
        public PlatformImage PlatformImage { get; }

        public ImageFormat ImageFormat { get; }

        public DiscordImage(IImage image, ImageFormat type)
        {
            if (image != null)
            {
                PlatformImage = image.ToPlatformImage() as PlatformImage;
                ImageFormat = type;
            }
        }

        public static DiscordImage CreateFrom(byte[] bytes, ImageFormat format)
        {
            return new DiscordImage(
                PlatformImage.FromStream(new MemoryStream(bytes)) as PlatformImage,
                format
            );
        }

        public static async Task<DiscordImage> CreateFrom(string url)
        {
            using HttpClient hc = new();
            using var response = await hc.GetAsync(url);
            response.EnsureSuccessStatusCode();
            using var stream = await response.Content.ReadAsStreamAsync();
            ImageFormat imageFormat = Enum.Parse<ImageFormat>(response.Content.Headers.First(x => x.Key == "Content-Type").Value.First().Replace("image/", string.Empty), true);
            return DiscordImage.CreateFrom(response.Content.ReadAsByteArrayAsync().Result, imageFormat);
        }

        public override string ToString()
        {
            if (PlatformImage == null)
                return null;

            string type = ImageFormat switch
            {
                ImageFormat.Jpeg => "jpeg",
                ImageFormat.Png => "png",
                ImageFormat.Gif => "gif",
                _ => throw new NotSupportedException("File extension not supported")
            };

            return $"data:image/{type};base64,{Convert.ToBase64String(PlatformImage.Bytes)}";
        }
    }
}

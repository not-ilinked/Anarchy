using System;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using Newtonsoft.Json;

namespace Discord
{
    internal class ImageJsonConverter : JsonConverter<DiscordImage>
    {
        public override bool CanRead => false;

        public override DiscordImage ReadJson(JsonReader reader, Type objectType, DiscordImage existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, DiscordImage image, JsonSerializer serializer)
        {
            writer.WriteValue($"data:{image.ImageFormat.ToMediaType()};base64,{Convert.ToBase64String(image.PlatformImage.Bytes)}");
        }
    }

    [JsonConverter(typeof(ImageJsonConverter))]
    public class DiscordImage
    {
        public DiscordImage(IImage image, ImageFormat type)
        {
            PlatformImage = image.ToPlatformImage() as PlatformImage;
            ImageFormat = type;
        }

        public PlatformImage PlatformImage { get; }

        public ImageFormat ImageFormat { get; }

        public static implicit operator DiscordAttachmentFile(DiscordImage image)
        {
            return new DiscordAttachmentFile(image.PlatformImage.Bytes, image.ImageFormat);
        }
    }
}

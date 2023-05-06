using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;

namespace Discord
{
    internal class ImageJsonConverter : JsonConverter<DiscordImage>
    {
        public override void Write(Utf8JsonWriter writer, DiscordImage value, JsonSerializerOptions options)
        {
            writer.WriteStringValue($"data:{value.ImageFormat.ToMediaType()};base64,{Convert.ToBase64String(value.PlatformImage.Bytes)}");
        }

        public override DiscordImage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
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
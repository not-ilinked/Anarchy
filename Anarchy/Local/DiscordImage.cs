using System;
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
        public DiscordImage(IImage image, ImageFormat type)
        {
            PlatformImage = image.ToPlatformImage() as PlatformImage;
            ImageFormat = type;
        }

        public PlatformImage PlatformImage { get; }

        public ImageFormat ImageFormat { get; }

        public static implicit operator DiscordAttachmentFile(DiscordImage image)
        {
            return new DiscordAttachmentFile(image.PlatformImage.Bytes, "image/" + image.ImageFormat);
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

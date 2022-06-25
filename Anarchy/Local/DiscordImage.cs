using System;
using System.IO;
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

    public enum ImageType
    {
        Png,
        Gif,
        Jpeg,
    }

    [JsonConverter(typeof(ImageJsonConverter))]
    public class DiscordImage
    {
        public PlatformImage Image { get; }

        public ImageType Type { get; }

        public DiscordImage(IImage image, ImageType type)
        {
            if (image != null)
            {
                Image = image.ToPlatformImage() as PlatformImage;
                Type = type;
            }
        }

        public DiscordImage(byte[] bytes, ImageType type)
        {
            if (bytes.Length > 0)
            {
                Image = PlatformImage.FromStream(new MemoryStream(bytes)) as PlatformImage;
                Type = type;
            }
        }

        public override string ToString()
        {
            if (Image == null)
                return null;

            string type = Type switch
            {
                ImageType.Jpeg => "jpeg",
                ImageType.Png => "png",
                ImageType.Gif => "gif",
                _ => throw new NotSupportedException("File extension not supported")
            };

            return $"data:image/{type};base64,{Convert.ToBase64String(Image.Bytes)}";
        }
    }
}

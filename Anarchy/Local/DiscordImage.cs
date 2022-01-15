using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Discord
{
    internal class ImageJsonConverter : JsonConverter
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
        public Image Image { get; private set; }

        public DiscordImage(Image image)
        {
            Image = image;
        }

        public override string ToString()
        {
            if (Image == null)
            {
                return null;
            }

            string type;

            if (ImageFormat.Jpeg.Equals(Image.RawFormat))
            {
                type = "jpeg";
            }
            else if (ImageFormat.Png.Equals(Image.RawFormat))
            {
                type = "png";
            }
            else if (ImageFormat.Gif.Equals(Image.RawFormat))
            {
                type = "gif";
            }
            else
            {
                throw new NotSupportedException("File extension not supported");
            }

            return $"data:image/{type};base64,{Convert.ToBase64String((byte[])new ImageConverter().ConvertTo(Image, typeof(byte[])))}";
        }

        public static implicit operator DiscordImage(Image instance)
        {
            return new DiscordImage(instance);
        }
    }
}

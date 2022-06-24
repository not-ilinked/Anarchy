using System;
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
        public byte[] Bytes { get; }

        public ImageType Type { get; }

        public DiscordImage(byte[] bytes, ImageType imageType)
        {
            Bytes = bytes;
            Type = imageType;
        }

        public override string ToString()
        {
            if (Bytes == null || Bytes.Length == 0)
            {
                return null;
            }

            string type = Type switch
            {
                ImageType.Jpeg => "jpeg",
                ImageType.Png => "png",
                ImageType.Gif => "gif",
                _ => throw new NotSupportedException("File extension not supported")
            };

            return $"data:image/{type};base64,{Convert.ToBase64String(Bytes)}";
        }
    }
}

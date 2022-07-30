using System;
using Microsoft.Maui.Graphics;

namespace Discord
{
    public static class DiscordImageMediaType
    {
        public static string ToMediaType(this ImageFormat _this)
        {
            return _this switch
            {
                ImageFormat.Jpeg => MediaTypeNames.Image.Jpeg,
                ImageFormat.Png => "image/png",
                ImageFormat.Gif => MediaTypeNames.Image.Gif,
                _ => throw new NotSupportedException("ImageFormat not supported.")
            };
        }

        public static ImageFormat ToImageFormat(string mediaType)
        {
            return Enum.Parse<ImageFormat>(mediaType.Replace("image/", string.Empty), true);
        }

        public static bool IsSupportedImageFormat(string mediaType)
        {
            string[] supportedImageTypes = { MediaTypeNames.Image.Png, MediaTypeNames.Image.Jpeg, MediaTypeNames.Image.Gif };

            foreach (var sit in supportedImageTypes)
                if (mediaType == sit)
                    return true;

            return false;
        }
    }
}

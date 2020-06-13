using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    public static class DiscordImage
    {
        public static string FromImage(Image image)
        {
            if (image == null)
                return null;

            string type;

            if (ImageFormat.Jpeg.Equals(image.RawFormat))
                type = "jpeg";
            else if (ImageFormat.Png.Equals(image.RawFormat))
                type = "png";
            else if (ImageFormat.Gif.Equals(image.RawFormat))
                type = "gif";
            else
                throw new NotSupportedException("File extension not supported");

            return $"data:image/{type};base64,{Convert.ToBase64String((byte[])new ImageConverter().ConvertTo(image, typeof(byte[])))}";
        }


        public static Image ToImage(string str)
        {
            if (str == null)
                return null;

            MemoryStream stream = new MemoryStream(Convert.FromBase64String(str.Split(',')[1]));

            return Image.FromStream(stream);
        }
    }
}

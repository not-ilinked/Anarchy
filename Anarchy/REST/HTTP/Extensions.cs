using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Discord
{
    internal static class Extensions
    {
        internal static MultipartFormDataContent MultipartFormData(this MessageProperties _this, string json)
        {
            var mpfd = new MultipartFormDataContent();

            for (int i = 0; i < _this.Attachments.Count; ++i)
            {
                var a = _this.Attachments[i];

                var ms = new MemoryStream();
                a.Image.PlatformImage.Save(ms, a.Image.ImageFormat);
                ms.Position = 0;

                var fileContent = new StreamContent(ms);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/" + a.Image.ImageFormat);

                mpfd.Add(fileContent, "files[0]", a.FileName);
            }

            if (!string.IsNullOrEmpty(json))
            {
                var jsonContent = new StringContent(json, null, null);
                jsonContent.Headers.Remove("Content-Type");
                mpfd.Add(jsonContent, "\"" + "payload_json" + "\"");
            }

            return mpfd;
        }
    }
}

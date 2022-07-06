using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Discord
{
    internal static class Extensions
    {
        internal static HttpContent MultipartFormData(this MessageProperties _this, string json)
        {
            HttpContent content;

            if (_this.Attachments != null && _this.Attachments.Count > 0)
            {
                var mpfc = new MultipartFormDataContent();

                for (int i = 0; i < _this.Attachments.Count; ++i)
                {
                    var a = _this.Attachments[i];

                    var ms = new MemoryStream();
                    a.Image.PlatformImage.Save(ms, a.Image.ImageFormat);
                    ms.Position = 0;

                    var fileContent = new StreamContent(ms);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/" + a.Image.ImageFormat);

                    mpfc.Add(fileContent, "files[0]", a.FileName);
                }

                if (!string.IsNullOrEmpty(json))
                {
                    var jsonContent = new StringContent(json, null, null);
                    jsonContent.Headers.Remove("Content-Type");
                    mpfc.Add(jsonContent, "\"" + "payload_json" + "\"");
                }

                content = mpfc;
            }
            else
                content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");

            return content;
        }
    }
}

using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Discord
{
    internal static class Extensions
    {
        internal static HttpContent MultipartFormData(this IDiscordAttachmentFileProvider provider, string json)
        {
            HttpContent content;

            var attachments = provider.GetAttachmentFiles();

            if (attachments != null && attachments.Any())
            {
                var mpfc = new MultipartFormDataContent();

                foreach (var a in attachments)
                {
                    var sc = new StreamContent(new MemoryStream(a.File.Bytes));
                    if (!string.IsNullOrEmpty(a.File.MediaType))
                        sc.Headers.ContentType = new MediaTypeHeaderValue(a.File.MediaType);

                    mpfc.Add(sc, $"files[{a.Id}]", Path.GetFileName(a.FileName));
                }

                if (!string.IsNullOrEmpty(json))
                {
                    var jsonContent = new StringContent(json, null, null);
                    jsonContent.Headers.Remove("Content-Type");
                    mpfc.Add(jsonContent, "\"payload_json\"");
                }

                content = mpfc;
            }
            else
                content = new StringContent(json, Encoding.UTF8, "application/json");

            return content;
        }
    }
}

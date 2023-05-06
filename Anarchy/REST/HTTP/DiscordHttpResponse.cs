using System.Collections.Generic;
using System.Text.Json;

namespace Discord
{
    public class DiscordHttpResponse
    {
        public int StatusCode { get; private set; }
        public JsonElement Body { get; private set; }
        public DiscordHttpResponse(int statusCode, string content)
        {
            StatusCode = statusCode;
            if (content != null && content.Length != 0)
                Body = JsonSerializer.Deserialize<JsonElement>(content);
        }

        public T Deserialize<T>()
        {
            return JsonSerializer.Deserialize<T>(Body.GetRawText());
        }
    }
}

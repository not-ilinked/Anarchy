using System.Net.Http;
using System.Text.Json;

namespace Discord
{
    internal static class DiscordHttpUtil
    {
        public static string BuildBaseUrl(uint apiVersion, DiscordReleaseChannel releaseChannel) =>
            $"https://{(releaseChannel == DiscordReleaseChannel.Stable ? "" : releaseChannel.ToString().ToLower() + ".")}discord.com/api/v{apiVersion}";

        public static void ValidateResponse(HttpResponseMessage response)
        {
            string content = response.Content.ReadAsStringAsync().Result;
            JsonElement body = content != null && content.Length != 0 ? JsonDocument.Parse(content).RootElement : default;

            ValidateResponse(response, body);
        }

        public static void ValidateResponse(HttpResponseMessage response, JsonElement body)
        {
            int statusCode = (int) response.StatusCode;

            if (statusCode >= 400)
            {
                if (statusCode == 429)
                    throw new RateLimitException(body.GetProperty("retry_after").GetInt32());
                else
                    throw new DiscordHttpException(body.Deserialize<DiscordHttpError>());
            }
        }
    }
}
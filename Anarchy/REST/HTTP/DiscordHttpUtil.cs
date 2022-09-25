using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Discord
{
    internal static class DiscordHttpUtil
    {
        public static string BuildBaseUrl(uint apiVersion, DiscordReleaseChannel releaseChannel) =>
            $"https://{(releaseChannel == DiscordReleaseChannel.Stable ? "" : releaseChannel.ToString().ToLower() + ".")}discord.com/api/v{apiVersion}";

        public static void ValidateResponse(HttpResponseMessage response)
        {
            string content = response.Content.ReadAsStringAsync().Result;
            JToken body = (content != null && content.Length != 0) ? JToken.Parse(content) : null;

            ValidateResponse(response, body);
        }

        public static void ValidateResponse(HttpResponseMessage response, JToken body)
        {
            int statusCode = (int) response.StatusCode;

            if (statusCode >= 400)
            {
                if (statusCode == 429)
                    throw new RateLimitException(body.Value<int>("retry_after"));
                else
                    throw new DiscordHttpException(body.ToObject<DiscordHttpError>());
            }
        }
    }
}

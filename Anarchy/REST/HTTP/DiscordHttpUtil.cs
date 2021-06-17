using Newtonsoft.Json.Linq;

namespace Discord
{
    internal static class DiscordHttpUtil
    {
        public static string BuildBaseUrl(int apiVersion, string domain) => $"https://{domain}/api/v{apiVersion}";

        public static void ValidateResponse(int statusCode, JToken body)
        {
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

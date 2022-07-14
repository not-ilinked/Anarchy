using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PuppeteerSharp;

namespace Discord
{
    public static class DiscordPuppeteer
    {
        private static Page _page;
        private static string _superProps;

        public static async Task StartAsync()
        {
            await new BrowserFetcher().DownloadAsync();
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true, IgnoreHTTPSErrors = true, Args = new string[] { "--no-sandbox", "--disable-web-security" } });
            _page = await browser.NewPageAsync();
            await _page.GoToAsync("https://discord.com");

            var properties = new SuperProperties();
            await _page.SetUserAgentAsync(properties.UserAgent);
            _superProps = properties.ToBase64();
        }

        public static void Start() => StartAsync().GetAwaiter().GetResult();


        public static async Task<T> CallAsync<T>(DiscordClient parentClient, string method, string url, object data = null)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "Authorization", parentClient.Token }
            };

            headers["X-Super-Properties"] = _superProps;

            if (method == "POST")
            {
                headers["Content-Type"] = "application/json";

                if (url.Contains("/invites/"))
                {
                    string invCode = url.Split('/').Last();

                    var invite = await CallAsync<GuildInvite>(parentClient, "GET", parentClient.HttpClient.BaseUrl + "/invites/" + invCode + $"?inputValue={invCode}&with_counts=true&with_expiration=true");
                    headers["X-Context-Properties"] = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{{\"location\":\"Join Guild\",\"location_guild_id\":\"{invite.Guild.Id}\",\"location_channel_id\":\"{invite.Channel.Id}\",\"location_channel_type\":{(int)invite.Channel.Type}}}"));
                }
            }

            string expression = "async() => {" + $@"const req = await fetch('{url}'," + "{" + $@"
method: '{method}',
headers: " + "{";

            foreach (var header in headers)
                expression += $@"'{header.Key}': '{header.Value}',";
            expression += @"},
";
            if (data != null)
                expression += $"body: '{JsonConvert.SerializeObject(data)}',";
            expression += "}); return {status: req.status, body: await req.json()};}";

            var result = await _page.EvaluateFunctionAsync<DiscordResponse>(expression);

            if (result.Status >= 400)
            {
                if (result.Status == 429) throw new RateLimitException(result.Body.Value<int>("retry_after"));
                else throw new DiscordHttpException(result.Body.ToObject<DiscordHttpError>());
            }

            return result.Body.ToObject<T>();
        }


        public static async Task<GuildInvite> JoinGuildAsync(DiscordClient client, string invCode, string captchaKey = null)
        {
            return (await CallAsync<GuildInvite>(client, "POST", client.HttpClient.BaseUrl + "/invites/" + invCode, captchaKey != null ? $"{{\"captcha_key\":\"{captchaKey}\"}}" : null)).SetClient(client);
        }

        public static GuildInvite JoinGuild(DiscordClient client, string invCode, string captchaKey = null)
        {
            return JoinGuildAsync(client, invCode, captchaKey).GetAwaiter().GetResult();
        }
    }
}

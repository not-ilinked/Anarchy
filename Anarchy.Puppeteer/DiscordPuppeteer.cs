using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAParser;

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

            string ua = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.77 Safari/537.36";

            await _page.SetUserAgentAsync(ua);

            var parser = Parser.GetDefault();
            var userBrowser = parser.ParseUserAgent(ua);
            var os = parser.ParseOS(ua);

            var properties = new DiscordConfig().SuperProperties;
            properties.Browser = userBrowser.Family;
            properties.BrowserVersion = $"{userBrowser.Major}.{userBrowser.Minor}.{userBrowser.Patch}.77";
            properties.OS = os.Family;
            properties.OSVersion = os.Major; // this could be better
            properties.UserAgent = ua;

            _superProps = properties.ToBase64();
        }

        public static void Start() => StartAsync().GetAwaiter().GetResult();


        public static async Task<T> CallAsync<T>(DiscordClient parentClient, string method, string url, object data = null)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "Authorization", parentClient.Token }
            };

            if (method == "POST")
            {
                headers["Content-Type"] = "application/json";
                headers["X-Super-Properties"] = _superProps;

                if (url.Contains("/invites/"))
                {
                    var invite = await CallAsync<GuildInvite>(parentClient, "GET", parentClient.HttpClient.BaseUrl + "/invites/" + url.Split('/').Last());
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
                if (result.Status == 429) throw new RateLimitException(parentClient, result.Body.Value<int>("retry_after"));
                else throw new DiscordHttpException(result.Body.ToObject<DiscordHttpError>());
            }

            return result.Body.ToObject<T>();
        }


        public static async Task<GuildInvite> JoinGuildAsync(DiscordClient client, string invCode)
        {
            return (await CallAsync<GuildInvite>(client, "POST", client.HttpClient.BaseUrl + "/invites/" + invCode));
        }

        public static GuildInvite JoinGuild(DiscordClient client, string invCode)
        {
            return JoinGuildAsync(client, invCode).GetAwaiter().GetResult();
        }
    }
}

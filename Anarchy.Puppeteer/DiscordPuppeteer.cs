using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static async Task<JToken> CallAsync(DiscordClient parentClient, string method, string url, object data = null)
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
            expression += "}); return {status: req.status, body: req.status === 204 ? null : await req.json()};}";

            var result = await _page.EvaluateFunctionAsync<DiscordResponse>(expression);

            if (result.Status >= 400)
            {
                if (result.Status == 429) throw new RateLimitException(result.Body.Value<int>("retry_after"));
                else throw new DiscordHttpException(result.Body.ToObject<DiscordHttpError>());
            }

            return result.Body;
        }

        public static async Task<T> CallAsync<T>(DiscordClient parentClient, string method, string url, object data = null) => (await CallAsync(parentClient, method, url, data)).ToObject<T>();


        public static async Task<GuildInvite> JoinGuildAsync(DiscordClient client, string invCode)
        {
            return (await CallAsync<GuildInvite>(client, "POST", client.HttpClient.BaseUrl + "/invites/" + invCode)).SetClient(client);
        }

        public static GuildInvite JoinGuild(DiscordClient client, string invCode)
        {
            return JoinGuildAsync(client, invCode).GetAwaiter().GetResult();
        }


        public static async Task SendFriendRequestAsync(DiscordClient client, string username, uint discriminator)
        {
            await CallAsync(client, "POST", client.HttpClient.BaseUrl + "/users/@me/relationships", new { username, discriminator });
        }

        public static void SendFriendReqeuest(DiscordClient client, string username, uint discriminator) =>
            SendFriendRequestAsync(client, username, discriminator).GetAwaiter().GetResult();
    }
}

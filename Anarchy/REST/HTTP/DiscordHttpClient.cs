using Newtonsoft.Json.Linq;
using System.Threading;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Net.Http;
using Leaf.xNet;
using System.Net;
using Discord.Gateway;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Discord
{
    public class DiscordHttpClient
    {
        private readonly DiscordClient _discordClient;

        public string BaseUrl
        {
            get { return $"https://{_discordClient.Config.RestDomain}/api/v{_discordClient.Config.ApiVersion}"; }
        }


        public DiscordHttpClient(DiscordClient discordClient)
        {
            _discordClient = discordClient;
        }


        private void CheckResponse(DiscordHttpResponse response)
        {
            if (response.StatusCode >= 400)
            {
                if (response.StatusCode == 429)
                    throw new RateLimitException(_discordClient, response.Deserialize<JObject>().Value<int>("retry_after"));
                else if (response.StatusCode == 400)
                    throw new InvalidParametersException(response.Deserialize<Dictionary<string, List<string>>>());
                else
                    throw new DiscordHttpException(_discordClient, response.Deserialize<DiscordHttpError>());
            }
        }


        /// <summary>
        /// Sends an HTTP request and checks for errors
        /// </summary>
        /// <param name="method">HTTP method to use</param>
        /// <param name="endpoint">API endpoint (fx. /users/@me)</param>
        /// <param name="payload">JSON content</param>
        private async Task<DiscordHttpResponse> SendAsync(Leaf.xNet.HttpMethod method, string endpoint, object payload = null)
        {
            endpoint = BaseUrl + endpoint;

            string json = "{}";
            if (payload != null)
            {
                if (payload.GetType() == typeof(string))
                    json = (string)payload;
                else
                    json = JsonConvert.SerializeObject(payload);
            }

            bool hasData = method == Leaf.xNet.HttpMethod.POST || method == Leaf.xNet.HttpMethod.PATCH || method == Leaf.xNet.HttpMethod.PUT || method == Leaf.xNet.HttpMethod.DELETE;

            while (true)
            {
                try
                {
                    DiscordHttpResponse resp;

                    // C# is retarded as shit
                    ProxyClient proxy = null;

                    if (_discordClient.GetType() == typeof(DiscordSocketClient))
                    {
                        var client = (DiscordSocketClient)_discordClient;

                        if (client.Config != null)
                            proxy = client.Config.Proxy;
                    }
                    else
                        proxy = _discordClient.Config.Proxy;

                    if (proxy == null || proxy.Type == ProxyType.HTTP)
                    {
                        HttpClient client = new HttpClient(new HttpClientHandler() { Proxy = proxy == null ? null : new WebProxy(proxy.Host, proxy.Port) });
                        if (_discordClient.Token != null)
                            client.DefaultRequestHeaders.Add("Authorization", _discordClient.Token);

                        client.DefaultRequestHeaders.Add("X-Super-Properties", _discordClient.Config.SuperProperties.Base64);

                        var response = await client.SendAsync(new HttpRequestMessage() 
                        { 
                            Content = hasData ? new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json") : null, 
                            Method = new System.Net.Http.HttpMethod(method.ToString()), RequestUri = new Uri(endpoint) 
                        });

                        resp = new DiscordHttpResponse((int)response.StatusCode, response.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        HttpRequest msg = new HttpRequest
                        {
                            IgnoreProtocolErrors = true,
                            UserAgent = _discordClient.Config.UserAgent,
                            Authorization = _discordClient.Token
                        };

                        if (hasData)
                            msg.AddHeader(HttpHeader.ContentType, "application/json");

                        msg.AddHeader("X-Super-Properties", _discordClient.Config.SuperProperties.Base64);
                        if (proxy != null)
                            msg.Proxy = proxy;

                        var response = msg.Raw(method, endpoint, hasData ? new Leaf.xNet.StringContent(json) : null);

                        resp = new DiscordHttpResponse((int)response.StatusCode, response.ToString());
                    }

                    CheckResponse(resp);
                    return resp;
                }
                catch (RateLimitException ex)
                {
                    if (_discordClient.Config.RetryOnRateLimit)
                        Thread.Sleep(ex.RetryAfter);
                    else
                        throw;
                }
            }
        }


        public async Task<DiscordHttpResponse> GetAsync(string endpoint)
        {
            return await SendAsync(Leaf.xNet.HttpMethod.GET, endpoint);
        }


        public async Task<DiscordHttpResponse> PostAsync(string endpoint, object payload = null)
        {
            return await SendAsync(Leaf.xNet.HttpMethod.POST, endpoint, payload);
        }


        public async Task<DiscordHttpResponse> DeleteAsync(string endpoint, object payload = null)
        {
            return await SendAsync(Leaf.xNet.HttpMethod.DELETE, endpoint, payload);
        }


        public async Task<DiscordHttpResponse> PutAsync(string endpoint, object payload = null)
        {
            return await SendAsync(Leaf.xNet.HttpMethod.PUT, endpoint, payload);
        }


        public async Task<DiscordHttpResponse> PatchAsync(string endpoint, object payload = null)
        {
            return await SendAsync(Leaf.xNet.HttpMethod.PATCH, endpoint, payload);
        }
    }
}
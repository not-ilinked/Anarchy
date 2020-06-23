using Newtonsoft.Json.Linq;
using System.Threading;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Net.Http;
using Leaf.xNet;
using System.Net;

namespace Discord
{
    public class Response
    {
        private readonly string _content;

        public Response(string content)
        {
            _content = content;
        }


        public override string ToString()
        {
            return _content;
        }
    }

    public class DiscordHttpClient
    {
        private readonly DiscordClient _discordClient;

        public string Fingerprint { get; private set; }


        public DiscordHttpClient(DiscordClient discordClient)
        {
            _discordClient = discordClient;
        }


        public void UpdateFingerprint()
        {
            Fingerprint = Get("/experiments").Deserialize<JObject>().Value<string>("fingerprint");
        }


        private void CheckResponse(string resp, int statusCode)
        {
            if (statusCode >= 400)
            {
                if (statusCode == 429)
                    throw new RateLimitException(_discordClient, resp.ToString().Deserialize<RateLimit>());
                else if (statusCode == 400)
                {
                    var obj = resp.Deserialize<JObject>();

                    if (!obj.ContainsKey("code") || !obj.ContainsKey("message"))
                        throw new InvalidParametersException(_discordClient, resp.ToString());
                }

                throw new DiscordHttpException(_discordClient, resp.Deserialize<DiscordHttpError>());
            }
        }


        /// <summary>
        /// Sends an HTTP request and checks for errors
        /// </summary>
        /// <param name="method">HTTP method to use</param>
        /// <param name="endpoint">API endpoint (fx. /users/@me)</param>
        /// <param name="payload">JSON content</param>
        private Response Send(Leaf.xNet.HttpMethod method, string endpoint, object payload = null)
        {
            string json = "{}";

            if (payload != null)
            {
                if (payload.GetType() == typeof(string))
                    json = (string)payload;
                else
                    json = JsonConvert.SerializeObject(payload);
            }

            bool isEndpoint = !endpoint.StartsWith("http");

            if (isEndpoint)
                endpoint = _discordClient.Config.ApiBaseUrl + endpoint;

            bool hasData = method == Leaf.xNet.HttpMethod.POST || method == Leaf.xNet.HttpMethod.PATCH || method == Leaf.xNet.HttpMethod.PUT || method == Leaf.xNet.HttpMethod.DELETE;

            while (true)
            {
                try
                {
                    string resp;
                    int statusCode;

                    if (_discordClient.Config.Proxy == null || _discordClient.Config.Proxy.Type == ProxyType.HTTP)
                    {
                        HttpClient client = new HttpClient(new HttpClientHandler() { Proxy = _discordClient.Config.Proxy == null ? null : new WebProxy(_discordClient.Config.Proxy.Host, _discordClient.Config.Proxy.Port) });
                        if (_discordClient.Token != null)
                            client.DefaultRequestHeaders.Add("Authorization", _discordClient.Token);
                        if (Fingerprint != null)
                            client.DefaultRequestHeaders.Add("X-Fingerprint", Fingerprint);

                        client.DefaultRequestHeaders.Add("X-Super-Properties", _discordClient.Config.SuperProperties.Base64);

                        var response = client.SendAsync(new HttpRequestMessage() { Content = hasData ? new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json") : null, Method = new System.Net.Http.HttpMethod(method.ToString()), RequestUri = new Uri(endpoint) }).Result;

                        resp = response.Content.ReadAsStringAsync().Result;
                        statusCode = (int)response.StatusCode;
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
                        if (Fingerprint != null)
                            msg.AddHeader("X-Fingerprint", Fingerprint);

                        msg.AddHeader("X-Super-Properties", _discordClient.Config.SuperProperties.Base64);
                        if (_discordClient.Config.Proxy != null)
                            msg.Proxy = _discordClient.Config.Proxy;

                        var response = msg.Raw(method, endpoint, hasData ? new Leaf.xNet.StringContent(json) : null);
                        resp = response.ToString();
                        statusCode = (int)response.StatusCode;
                    }

                    CheckResponse(resp, statusCode);
                    return new Response(resp);
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


        public Response Get(string endpoint)
        {
            return Send(Leaf.xNet.HttpMethod.GET, endpoint);
        }


        public Response Post(string endpoint, object payload = null)
        {
            return Send(Leaf.xNet.HttpMethod.POST, endpoint, payload);
        }


        public Response Delete(string endpoint, object payload = null)
        {
            return Send(Leaf.xNet.HttpMethod.DELETE, endpoint, payload);
        }


        public Response Put(string endpoint, object payload = null)
        {
            return Send(Leaf.xNet.HttpMethod.PUT, endpoint, payload);
        }


        public Response Patch(string endpoint, object payload = null)
        {
            return Send(Leaf.xNet.HttpMethod.PATCH, endpoint, payload);
        }
    }
}
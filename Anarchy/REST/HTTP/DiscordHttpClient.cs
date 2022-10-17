using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Discord
{
    public class DiscordHttpClient
    {
        private readonly DiscordClient _discordClient;
        public string BaseUrl => DiscordHttpUtil.BuildBaseUrl(_discordClient.Config.ApiVersion, _discordClient.Config.SuperProperties.ReleaseChannel);

        public DiscordHttpClient(DiscordClient discordClient)
        {
            _discordClient = discordClient;
        }

        private string _anarchyVersion;
        private string AnarchyVersion
        {
            get
            {
                if (_anarchyVersion == null)
                {
                    Assembly assembly = Assembly.GetAssembly(typeof(DiscordHttpClient));
                    _anarchyVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
                }
                return _anarchyVersion;
            }
        }

        /// <summary>
        /// Sends an HTTP request and checks for errors
        /// </summary>
        /// <param name="method">HTTP method to use</param>
        /// <param name="endpoint">API endpoint (fx. /users/@me)</param>
        /// <param name="payload">JSON content</param>
        private async Task<DiscordHttpResponse> SendAsync(HttpMethod method, string endpoint, HttpContent content = null)
        {
            if (!endpoint.StartsWith("https"))
                endpoint = DiscordHttpUtil.BuildBaseUrl(_discordClient.Config.ApiVersion, _discordClient.Config.SuperProperties.ReleaseChannel) + endpoint;

            uint retriesLeft = _discordClient.Config.RestConnectionRetries;

            while (true)
            {
                try
                {
                    var client = _discordClient.Config.Proxy != null
                        ? new HttpClient(new HttpClientHandler() { Proxy = _discordClient.Config.Proxy })
                        : new HttpClient();

                    if (_discordClient.Token != null)
                        client.DefaultRequestHeaders.Add("Authorization", _discordClient.Token);

                    if (_discordClient.User != null && _discordClient.User.Type == DiscordUserType.Bot)
                        client.DefaultRequestHeaders.Add("User-Agent", $"Anarchy/{AnarchyVersion}");
                    else
                    {
                        client.DefaultRequestHeaders.Add("User-Agent", _discordClient.Config.SuperProperties.UserAgent);
                        client.DefaultRequestHeaders.Add("X-Super-Properties", _discordClient.Config.SuperProperties.ToBase64());
                    }

                    var response = await client.SendAsync(new HttpRequestMessage()
                    {
                        Content = content,
                        Method = new HttpMethod(method.ToString()),
                        RequestUri = new Uri(endpoint)
                    });

                    var discordResponse = new DiscordHttpResponse(
                        (int) response.StatusCode,
                        await response.Content.ReadAsStringAsync()
                    );
                    DiscordHttpUtil.ValidateResponse(response, discordResponse.Body);

                    return discordResponse;
                }
                catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
                {
                    if (retriesLeft == 0)
                        throw new DiscordConnectionException();

                    retriesLeft--;
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
            return await SendAsync(HttpMethod.Get, endpoint);
        }

        public async Task<DiscordHttpResponse> PostAsync(string endpoint, object payload = null)
        {
            return await SendAsync(HttpMethod.Post, endpoint, MakeStringContent(payload));
        }

        public async Task<DiscordHttpResponse> PostAsync(string endpoint, MessageProperties props)
        {
            return await SendAsync(HttpMethod.Post, endpoint, MakeMultipartFormDataContent(props));
        }

        public async Task<DiscordHttpResponse> DeleteAsync(string endpoint, object payload = null)
        {
            return await SendAsync(HttpMethod.Delete, endpoint, MakeStringContent(payload));
        }

        public async Task<DiscordHttpResponse> PutAsync(string endpoint, object payload = null)
        {
            return await SendAsync(HttpMethod.Put, endpoint, MakeStringContent(payload));
        }

        public async Task<DiscordHttpResponse> PatchAsync(string endpoint, object payload = null)
        {
            return await SendAsync(HttpMethod.Patch, endpoint, MakeStringContent(payload));
        }

        private static HttpContent MakeStringContent(object payload)
        {
            return new StringContent(MakeJson(payload), Encoding.UTF8, "application/json");
        }

        private static HttpContent MakeMultipartFormDataContent(MessageProperties props)
        {
            string json = MakeJson(props);
            HttpContent content;

            var attachments = props.GetAttachmentFiles();

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

        private static string MakeJson(object payload)
        {
            string json = "{}";
            if (payload != null)
            {
                if (payload.GetType() == typeof(string))
                    json = (string) payload;
                else
                    json = JsonConvert.SerializeObject(payload, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            }
            return json;
        }
    }
}
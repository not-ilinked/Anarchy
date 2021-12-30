using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Discord
{
    public class SuperProperties
    {
        private static int _versionCache;
        private static int GetClientVersion()
        {
            if (_versionCache == 0)
            {
                var client = new HttpClient();

                string appPage = client.GetStringAsync("https://discord.com/app").Result;
                const string findThis = "build_number:\"";

                foreach (var asset in Regex.Matches(appPage, "/assets/.{20}.js"))
                {
                    var content = client.GetStringAsync("https://discord.com" + asset).Result;

                    if (content.Contains(findThis))
                    {
                        string buildNumber = content.Substring(content.IndexOf(findThis) + findThis.Length).Split('"')[0];

                        _versionCache = int.Parse(buildNumber);
                        break;
                    }
                }
            }

            return _versionCache;
        }

        [JsonProperty("os")]
        public string OS { get; set; } = "Windows";

        [JsonProperty("browser")]
        public string Browser { get; set; } = "Chrome";

        [JsonProperty("device")]
        public string Device { get; set; } = "";

        [JsonProperty("system_locale")]
        public string SystemLocale { get; set; } = "da-DK";

        [JsonProperty("browser_user_agent")]
        public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) discord/1.0.9003 Chrome/91.0.4472.164 Electron/13.4.0 Safari/537.36";

        [JsonProperty("browser_version")]
        public string BrowserVersion { get; set; } = "91.0.4472.106";

        [JsonProperty("os_version")]
        public string OSVersion { get; set; } = "10";

        [JsonProperty("referrer")]
        public string Referrer { get; set; } = "";

        [JsonProperty("referring_domain")]
        public string ReferrerDomain { get; set; } = "";

        [JsonProperty("referrer_current")]
        public string ReferrerCurrent { get; set; } = "";

        [JsonProperty("referring_domain_current")]
        public string ReferrerDomainCurrent { get; set; } = "";

        [JsonProperty("release_channel")]
        private string _relChannel = "stable";

        public DiscordReleaseChannel ReleaseChannel
        {
            get => (DiscordReleaseChannel)Enum.Parse(typeof(DiscordReleaseChannel), _relChannel, true);
            set => _relChannel = value.ToString().ToLower();
        }

        [JsonProperty("client_build_number")]
        public int ClientVersion { get; set; } = GetClientVersion();

        [JsonProperty("client_event_source")]
        public string EventSource { get; set; }


        public static SuperProperties FromBase64(string base64)
        {
            return JsonConvert.DeserializeObject<SuperProperties>(Encoding.UTF8.GetString(Convert.FromBase64String(base64)));
        }


        public string ToBase64()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this)));
        }


        public override string ToString()
        {
            return UserAgent;
        }
    }
}

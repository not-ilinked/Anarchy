using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
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

                var assets = new List<Match>(Regex.Matches(appPage, "/assets/.{20}.js"));
                assets.Reverse();
                foreach (var asset in assets)
                {
                    var content = client.GetStringAsync("https://discord.com" + asset).Result;

                    if (content.Contains(findThis))
                    {
                        string buildNumber = content[(content.IndexOf(findThis) + findThis.Length)..].Split('"')[0];

                        _versionCache = int.Parse(buildNumber);
                        break;
                    }
                }
            }

            return _versionCache;
        }

        [JsonPropertyName("os")]
        public string OS { get; set; } = "Windows";

        [JsonPropertyName("browser")]
        public string Browser { get; set; } = "Chrome";

        [JsonPropertyName("device")]
        public string Device { get; set; } = "";

        [JsonPropertyName("system_locale")]
        public string SystemLocale { get; set; } = "da-DK";

        [JsonPropertyName("browser_user_agent")]
        public string UserAgent { get; set; } = "Discord/31433 CFNetwork/1331.0.7 Darwin/21.4.0";

        [JsonPropertyName("browser_version")]
        public string BrowserVersion { get; set; } = "91.0.4472.106";

        [JsonPropertyName("os_version")]
        public string OSVersion { get; set; } = "10";

        [JsonPropertyName("referrer")]
        public string Referrer { get; set; } = "";

        [JsonPropertyName("referring_domain")]
        public string ReferrerDomain { get; set; } = "";

        [JsonPropertyName("referrer_current")]
        public string ReferrerCurrent { get; set; } = "";

        [JsonPropertyName("referring_domain_current")]
        public string ReferrerDomainCurrent { get; set; } = "";

        [JsonPropertyName("release_channel")]
        private string _relChannel = "stable";

        public DiscordReleaseChannel ReleaseChannel
        {
            get => (DiscordReleaseChannel) Enum.Parse(typeof(DiscordReleaseChannel), _relChannel, true);
            set => _relChannel = value.ToString().ToLower();
        }

        [JsonPropertyName("client_build_number")]
        public int ClientVersion { get; set; } = GetClientVersion();

        [JsonPropertyName("client_event_source")]
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

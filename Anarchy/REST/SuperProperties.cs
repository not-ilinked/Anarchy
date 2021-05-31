using Newtonsoft.Json;
using System;
using System.Text;

namespace Discord
{
    public class SuperProperties
    {
        [JsonProperty("os")]
        public string OS { get; set; }

        [JsonProperty("browser")]
        public string Browser { get; set; }

        [JsonProperty("device")]
        public string Device { get; set; }

        [JsonProperty("system_locale")]
        public string SystemLocale { get; set; }

        [JsonProperty("browser_user_agent")]
        public string UserAgent { get; set; }

        [JsonProperty("browser_version")]
        public string BrowserVersion { get; set; }

        [JsonProperty("os_version")]
        public string OSVersion { get; set; }

        [JsonProperty("referrer")]
        public string Referrer { get; set; }

        [JsonProperty("referring_domain")]
        public string ReferrerDomain { get; set; }

        [JsonProperty("referrer_current")]
        public string ReferrerCurrent { get; set; }

        [JsonProperty("referring_domain_current")]
        public string ReferrerDomainCurrent { get; set; }

        [JsonProperty("release_channel")]
        public string ReleaseChannel { get; set; }

        [JsonProperty("client_build_number")]
        public int ClientVersion { get; set; }

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
using Leaf.xNet;

namespace Discord
{
    public class DiscordConfig
    {
        public ProxyClient Proxy { get; set; }
        public string UserAgent
        {
            get { return SuperProperties.UserAgent; }
            set { SuperProperties.UserAgent = value; }
        }
        public SuperProperties SuperProperties { get; set; } = SuperProperties.FromBase64("eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiRGlzY29yZCBDbGllbnQiLCJyZWxlYXNlX2NoYW5uZWwiOiJzdGFibGUiLCJjbGllbnRfdmVyc2lvbiI6IjAuMC4zMDYiLCJvc192ZXJzaW9uIjoiMTAuMC4xODM2MiIsIm9zX2FyY2giOiJ4NjQiLCJjbGllbnRfYnVpbGRfbnVtYmVyIjo2MTkyMywiY2xpZW50X2V2ZW50X3NvdXJjZSI6bnVsbH0=");
        public bool GetFingerprint { get; set; } = true;
        public string ApiBaseUrl { get; set; } = "https://discord.com/api/v6";
        public bool RetryOnRateLimit { get; set; } = true;
    }
}

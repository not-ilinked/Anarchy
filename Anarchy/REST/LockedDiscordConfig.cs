using System.Net;

namespace Discord
{
    public class LockedDiscordConfig
    {
        public IWebProxy Proxy { get; private set; }
        public SuperProperties SuperProperties { get; private set; }
        public uint RestConnectionRetries { get; private set; }
        public uint ApiVersion { get; private set; }
        public bool RetryOnRateLimit { get; private set; }

        public LockedDiscordConfig(ApiConfig config)
        {
            Proxy = config.Proxy;
            SuperProperties = config.SuperProperties;
            RestConnectionRetries = config.RestConnectionRetries;
            ApiVersion = config.ApiVersion;
            RetryOnRateLimit = config.RetryOnRateLimit;
        }
    }
}

namespace Discord
{
    public class DiscordConfig
    {
        public AnarchyProxy Proxy { get; set; }
        public SuperProperties SuperProperties { get; set; } = SuperProperties.FromBase64("eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6ImRhLURLIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzkxLjAuNDQ3Mi43NyBTYWZhcmkvNTM3LjM2IiwiYnJvd3Nlcl92ZXJzaW9uIjoiOTEuMC40NDcyLjc3Iiwib3NfdmVyc2lvbiI6IjEwIiwicmVmZXJyZXIiOiIiLCJyZWZlcnJpbmdfZG9tYWluIjoiIiwicmVmZXJyZXJfY3VycmVudCI6IiIsInJlZmVycmluZ19kb21haW5fY3VycmVudCI6IiIsInJlbGVhc2VfY2hhbm5lbCI6InN0YWJsZSIsImNsaWVudF9idWlsZF9udW1iZXIiOjg2NzgyLCJjbGllbnRfZXZlbnRfc291cmNlIjpudWxsfQ==");
        public string RestDomain { get; set; } = "discord.com";
        public uint RestConnectionRetries { get; set; } = 0;
        public uint ApiVersion { get; set; } = 9;
        public bool RetryOnRateLimit { get; set; } = true;
    }
}

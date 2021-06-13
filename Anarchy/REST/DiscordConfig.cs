namespace Discord
{
    public class DiscordConfig
    {
        public AnarchyProxy Proxy { get; set; }
        public SuperProperties SuperProperties { get; set; } = SuperProperties.FromBase64("eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiQ2hyb21lIiwiZGV2aWNlIjoiIiwic3lzdGVtX2xvY2FsZSI6ImRhLURLIiwiYnJvd3Nlcl91c2VyX2FnZW50IjoiTW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzkxLjAuNDQ3Mi4xMDEgU2FmYXJpLzUzNy4zNiIsImJyb3dzZXJfdmVyc2lvbiI6IjkxLjAuNDQ3Mi4xMDEiLCJvc192ZXJzaW9uIjoiMTAiLCJyZWZlcnJlciI6IiIsInJlZmVycmluZ19kb21haW4iOiIiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6ODc3MDksImNsaWVudF9ldmVudF9zb3VyY2UiOm51bGx9");
        public string RestDomain { get; set; } = "discord.com";
        public uint RestConnectionRetries { get; set; } = 0;
        public uint ApiVersion { get; set; } = 9;
        public bool RetryOnRateLimit { get; set; } = true;
    }
}

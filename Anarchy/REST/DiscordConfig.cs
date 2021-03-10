namespace Discord
{
    public class DiscordConfig
    {
        public AnarchyProxy Proxy { get; set; }
        public SuperProperties SuperProperties { get; set; } = SuperProperties.FromBase64("eyJvcyI6IldpbmRvd3MiLCJicm93c2VyIjoiRmlyZWZveCIsImRldmljZSI6IiIsImJyb3dzZXJfdXNlcl9hZ2VudCI6Ik1vemlsbGEvNS4wIChXaW5kb3dzIE5UIDEwLjA7IFdpbjY0OyB4NjQ7IHJ2OjgxLjApIEdlY2tvLzIwMTAwMTAxIEZpcmVmb3gvODEuMCIsImJyb3dzZXJfdmVyc2lvbiI6IjgxLjAiLCJvc192ZXJzaW9uIjoiMTAiLCJyZWZlcnJlciI6IiIsInJlZmVycmluZ19kb21haW4iOiIiLCJyZWZlcnJlcl9jdXJyZW50IjoiIiwicmVmZXJyaW5nX2RvbWFpbl9jdXJyZW50IjoiIiwicmVsZWFzZV9jaGFubmVsIjoic3RhYmxlIiwiY2xpZW50X2J1aWxkX251bWJlciI6Njg1NTgsImNsaWVudF9ldmVudF9zb3VyY2UiOm51bGx9");
        public string RestDomain { get; set; } = "discord.com";
        public uint RestConnectionRetries { get; set; } = 0;
        public uint ApiVersion { get; set; } = 8;
        public bool RetryOnRateLimit { get; set; } = true;
    }
}

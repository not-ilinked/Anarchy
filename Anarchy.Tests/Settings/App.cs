namespace Discord.Settings
{
    internal class App
    {
        public string Token { get; set; } = string.Empty;
        public ulong ChannelId { get; set; }
        public ProxySettings? Proxy { get; set; }
    }
}

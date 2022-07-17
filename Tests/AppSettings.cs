namespace Discord
{
    internal class AppSettings
    {
        public string Token { get; set; }
        public ulong ChannelId { get; set; }

        public AppSettings()
        {
            Token = string.Empty;
        }
    }
}

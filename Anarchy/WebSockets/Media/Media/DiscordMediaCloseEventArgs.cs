using Discord.WebSockets;

namespace Discord.Media
{
    public class DiscordMediaCloseEventArgs : DiscordWebSocketCloseEventArgs<DiscordMediaCloseCode>
    {
        public DiscordMediaCloseEventArgs(DiscordMediaCloseCode code, string reason) : base(code, reason)
        { }
    }
}

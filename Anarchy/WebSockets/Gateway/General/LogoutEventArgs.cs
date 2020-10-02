using Discord.WebSockets;

namespace Discord.Gateway
{
    public class LogoutEventArgs : DiscordWebSocketCloseEventArgs<GatewayCloseCode>
    {
        public LogoutEventArgs(GatewayCloseCode error, string reason) : base(error, reason)
        { }
    }
}

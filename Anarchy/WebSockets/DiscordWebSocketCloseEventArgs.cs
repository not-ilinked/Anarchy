using System;

namespace Discord.WebSockets
{
    public class DiscordWebSocketCloseEventArgs : EventArgs
    {
        public int Code { get; private set; }
        public string Reason { get; private set; }

        public DiscordWebSocketCloseEventArgs(int code, string reason)
        {
            Code = code;
            Reason = reason;
        }
    }
}

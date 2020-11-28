using System;

namespace Discord.WebSockets
{
    public class DiscordWebSocketCloseEventArgs<TCloseCode> : EventArgs where TCloseCode : Enum
    {
        public TCloseCode Code { get; private set; }
        public string Reason { get; private set; }

        public DiscordWebSocketCloseEventArgs(TCloseCode code, string reason)
        {
            Code = code;
            Reason = reason;
        }
    }
}

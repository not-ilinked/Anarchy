using System;

namespace Discord.Gateway
{
    public class LogoutEventArgs : EventArgs
    {
        public bool Clean { get; private set; }
        public GatewayCloseError Error { get; private set; }

        public LogoutEventArgs()
        {
            Clean = true;
        }

        public LogoutEventArgs(GatewayCloseError error)
        {
            Error = error;
        }
    }
}

using System;

namespace Discord.Gateway
{
    public class LogoutEventArgs : EventArgs
    {
        public bool HasError { get; private set; }
        public GatewayCloseError Error { get; private set; }

        public LogoutEventArgs() { }

        public LogoutEventArgs(GatewayCloseError error)
        {
            HasError = true;
            Error = error;
        }
    }
}

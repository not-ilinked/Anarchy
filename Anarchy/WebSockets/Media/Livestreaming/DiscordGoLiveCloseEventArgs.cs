using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Media
{
    public class DiscordGoLiveCloseEventArgs : DiscordMediaCloseEventArgs
    {
        public DiscordGoLiveError? Error { get; private set; }
        public string RawError { get; private set; }

        internal DiscordGoLiveCloseEventArgs(DiscordMediaCloseCode code, string reason, GoLiveDelete delete = null) : base(code, reason)
        { 
            if (delete != null)
            {
                Error = delete.Reason;
                RawError = delete.RawReason;
            }
        }
    }
}

using System;

namespace Discord.Media
{
    public class GoLiveDisconnectEventArgs : EventArgs
    {
        public ulong StreamerId { get; }

        public string RawReason { get; }
        public DiscordGoLiveError Reason { get; }

        internal GoLiveDisconnectEventArgs(ulong streamerId, GoLiveDelete goLive)
        {
            StreamerId = streamerId;

            RawReason = goLive.RawReason;

            if (Enum.TryParse(RawReason.Replace("_", ""), true, out DiscordGoLiveError err)) Reason = err;
            else Reason = DiscordGoLiveError.Unknown;
        }
    }
}

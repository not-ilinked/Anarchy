using System;

namespace Discord.Media
{
    public class LivestreamDisconnectEventArgs : EventArgs
    {
        public ulong StreamerId { get; }

        public string RawReason { get; }
        public DiscordLivestreamError Reason { get; }

        internal LivestreamDisconnectEventArgs(ulong streamerId, GoLiveDelete goLive)
        {
            StreamerId = streamerId;

            RawReason = goLive.RawReason;

            if (Enum.TryParse(RawReason.Replace("_", ""), true, out DiscordLivestreamError err)) Reason = err;
            else Reason = DiscordLivestreamError.Unknown;
        }
    }
}

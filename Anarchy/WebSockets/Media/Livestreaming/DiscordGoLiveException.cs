using System;

namespace Discord.Media
{
    public class DiscordGoLiveException : Exception
    {
        public DiscordGoLiveError Error { get; private set; }
        public string RawError { get; private set; }

        internal DiscordGoLiveException(GoLiveDelete delete) : base($"Failed to connect to Go Live. Error: {delete.Reason}")
        {
            Error = delete.Reason;
            RawError = delete.RawReason;
        }
    }
}

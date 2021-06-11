using Newtonsoft.Json;
using System;

namespace Discord.Media
{
    internal class GoLiveDelete : GoLiveStreamKey
    {
        // stream_not_found, stream_ended
        [JsonProperty("reason")]
        public string RawReason { get; private set; }
        /*
        public DiscordGoLiveError Reason
        {
            get
            {
                if (Enum.TryParse(RawReason.Replace("_", ""), true, out DiscordGoLiveError err))
                    return err;
                else
                    return DiscordGoLiveError.Unknown; 
            }
        }*/
    }
}
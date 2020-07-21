using Newtonsoft.Json;
using System;

namespace Discord.Voice
{
    public class DiscordSpeakingStateEventArgs : EventArgs
    {
        [JsonProperty("user_id")]
        public ulong UserId { get; internal set; }


        [JsonProperty("ssrc")]
        public uint SSRC { get; private set; }


        [JsonProperty("speaking")]
        public DiscordVoiceSpeakingState State { get; private set; }
    }
}

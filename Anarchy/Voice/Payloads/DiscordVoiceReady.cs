using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord.Voice
{
    public class DiscordVoiceReady
    {
        [JsonProperty("ssrc")]
        public int SSRC { get; private set; }


        [JsonProperty("ip")]
        public string IP { get; private set; }


        [JsonProperty("port")]
        public int Port { get; private set; }


        [JsonProperty("modes")]
        public List<string> EncryptionModes { get; private set; }
    }
}

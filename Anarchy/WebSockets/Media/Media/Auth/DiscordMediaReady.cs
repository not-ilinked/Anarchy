using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord.Media
{
    internal class DiscordMediaReady
    {
        [JsonProperty("ssrc")]
        public uint SSRC { get; private set; }


        [JsonProperty("ip")]
        public string IP { get; private set; }


        [JsonProperty("port")]
        public int Port { get; private set; }


        [JsonProperty("modes")]
        public List<string> EncryptionModes { get; private set; }


        [JsonProperty("streams")]
        public List<StreamSSRC> Streams { get; private set; }
    }
}

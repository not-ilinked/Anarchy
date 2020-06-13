using Newtonsoft.Json;

namespace Discord.Voice
{
    public class DiscordVoiceProtocolData
    {
        [JsonProperty("address")]
        public string Host { get; set; }


        [JsonProperty("port")]
        public int Port { get; set; }


        [JsonProperty("mode")]
        public string EncryptionMode { get; set; }
    }
}

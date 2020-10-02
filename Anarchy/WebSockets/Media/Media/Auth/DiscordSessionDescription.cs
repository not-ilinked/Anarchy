using Newtonsoft.Json;

namespace Discord.Media
{
    internal class DiscordSessionDescription : MediaCodecSelection
    {
        [JsonProperty("media_session_id")]
        public string SessionId { get; private set; }


        [JsonProperty("mode")]
        public string EncryptionMode { get; private set; }


        [JsonProperty("secret_key")]
        public byte[] SecretKey { get; private set; }
    }
}



using System.Text.Json.Serialization;
namespace Discord.Media
{
    internal class DiscordSessionDescription : MediaCodecSelection
    {
        [JsonPropertyName("media_session_id")]
        public string SessionId { get; private set; }

        [JsonPropertyName("mode")]
        public string EncryptionMode { get; private set; }

        [JsonPropertyName("secret_key")]
        public byte[] SecretKey { get; private set; }
    }
}



using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    internal class GatewayResume
    {
        // This ctor isn't used anywhere, it's just c# generics being autistic /shrug
        public GatewayResume() { }

        internal GatewayResume(DiscordSocketClient client)
        {
            Token = client.Token;
            SessionId = client.SessionId;
            Sequence = client.Sequence;
        }

        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("session_id")]
        public string SessionId { get; set; }

        [JsonPropertyName("seq")]
        public uint? Sequence { get; set; }
    }
}

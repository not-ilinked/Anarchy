using Newtonsoft.Json;

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

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("session_id")]
        public string SessionId { get; set; }

        [JsonProperty("seq")]
        public uint? Sequence { get; set; }
    }
}

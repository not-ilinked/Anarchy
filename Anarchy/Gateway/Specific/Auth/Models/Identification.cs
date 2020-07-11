using Newtonsoft.Json;

namespace Discord.Gateway
{
    internal class GatewayIdentification
    {
        [JsonProperty("token")]
        public string Token { get; set; }


        [JsonProperty("properties")]
        public SuperProperties Properties { get; set; }


        [JsonProperty("presence")]
        public PresenceChange Presence { get; set; }


        [JsonProperty("compress")]
        public bool Compress { get; set; }


        [JsonProperty("intents")]
        public DiscordGatewayIntent? Intents { get; set; }
        

        public bool ShouldSerializeIntents()
        {
            return Intents.HasValue;
        }


        public override string ToString()
        {
            return Token;
        }
    }
}

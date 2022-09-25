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
        public PresenceProperties Presence { get; set; }

        [JsonProperty("compress")]
        public bool Compress { get; set; }

        [JsonProperty("intents")]
        public DiscordGatewayIntent? Intents { get; set; }

        public bool ShouldSerializeIntents()
        {
            return Intents.HasValue;
        }

        [JsonProperty("shard")]
        private uint[] _shard
        {
            get { return new uint[] { Shard.Index, Shard.Total }; }
        }

        [JsonIgnore]
        public DiscordShard Shard { get; set; }

        public bool ShouldSerialize_shard()
        {
            return Shard != null;
        }

        public override string ToString()
        {
            return Token;
        }
    }
}

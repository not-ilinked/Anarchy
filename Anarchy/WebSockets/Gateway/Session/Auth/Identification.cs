

using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    internal class GatewayIdentification
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("properties")]
        public SuperProperties Properties { get; set; }

        [JsonPropertyName("presence")]
        public PresenceProperties Presence { get; set; }

        [JsonPropertyName("compress")]
        public bool Compress { get; set; }

        [JsonPropertyName("intents")]
        public DiscordGatewayIntent? Intents { get; set; }

        public bool ShouldSerializeIntents()
        {
            return Intents.HasValue;
        }

        [JsonPropertyName("shard")]
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

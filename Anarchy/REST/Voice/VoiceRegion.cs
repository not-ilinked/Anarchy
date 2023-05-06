

using System.Text.Json.Serialization;

namespace Discord
{
    public class VoiceRegion
    {
        [JsonPropertyName("id")]
        public string Id { get; private set; }

        [JsonPropertyName("name")]
        public string Name { get; private set; }

        [JsonPropertyName("optimal")]
        public bool Optimal { get; private set; }

        [JsonPropertyName("vip")]
        public bool Premium { get; private set; }

        [JsonPropertyName("custom")]
        public bool Custom { get; private set; }

        [JsonPropertyName("deprecated")]
        public bool Depcrecated { get; private set; }

        public override string ToString()
        {
            return Name;
        }

        public static implicit operator string(VoiceRegion instance)
        {
            return instance.Id;
        }
    }
}

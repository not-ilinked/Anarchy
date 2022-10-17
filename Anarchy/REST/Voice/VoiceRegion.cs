using Newtonsoft.Json;

namespace Discord
{
    public class VoiceRegion
    {
        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("optimal")]
        public bool Optimal { get; private set; }

        [JsonProperty("vip")]
        public bool Premium { get; private set; }

        [JsonProperty("custom")]
        public bool Custom { get; private set; }

        [JsonProperty("deprecated")]
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

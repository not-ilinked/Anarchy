using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord
{
    public class ApplicationCommandProperties
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        public bool ShouldSerializeName() => Name != null;

        [JsonPropertyName("description")]
        public string Description { get; set; }

        public bool ShouldSerializeDescription() => Description != null;

        [JsonPropertyName("options")]
        public List<ApplicationCommandOption> Options { get; set; }

        public bool ShouldSerializeOptions() => Options != null;

        [JsonPropertyName("flags")]
        private int Flags => 64;

        public bool Ephemeral { get; set; }
        public bool ShouldSerializeFlags() => Ephemeral;
    }
}

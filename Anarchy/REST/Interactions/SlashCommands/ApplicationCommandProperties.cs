using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public class ApplicationCommandProperties
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        public bool ShouldSerializeName()
        {
            return Name != null;
        }

        [JsonProperty("description")]
        public string Description { get; set; }

        public bool ShouldSerializeDescription()
        {
            return Description != null;
        }

        [JsonProperty("options")]
        public List<ApplicationCommandOption> Options { get; set; }

        public bool ShouldSerializeOptions()
        {
            return Options != null;
        }

        [JsonProperty("flags")]
        private int Flags => 64;

        public bool Ephemeral { get; set; }
        public bool ShouldSerializeFlags()
        {
            return Ephemeral;
        }
    }
}

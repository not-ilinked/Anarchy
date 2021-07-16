using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public class SelectMenuComponent : MessageInputComponent
    {
        public SelectMenuComponent()
        {
            Type = MessageComponentType.Select;
        }

        [JsonProperty("options")]
        public List<SelectMenuOption> Options { get; set; }

        [JsonProperty("min_values")]
        public uint? MinimumSelected { get; set; }

        [JsonProperty("max_values")]
        public uint? MaxSelected { get; set; }

        [JsonProperty("placeholder")]
        public string Placeholder { get; set; }
    }
}

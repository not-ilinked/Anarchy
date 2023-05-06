using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord
{
    public class SelectMenuComponent : MessageInputComponent
    {
        public SelectMenuComponent()
        {
            Type = MessageComponentType.Select;
        }

        [JsonPropertyName("options")]
        public List<SelectMenuOption> Options { get; set; }

        [JsonPropertyName("min_values")]
        public uint? MinimumSelected { get; set; }

        [JsonPropertyName("max_values")]
        public uint? MaxSelected { get; set; }

        [JsonPropertyName("placeholder")]
        public string Placeholder { get; set; }
    }
}

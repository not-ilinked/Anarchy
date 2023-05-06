using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord
{
    public class ApplicationCommandOption
    {
        [JsonPropertyName("type")]
        public CommandOptionType Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }

        [JsonPropertyName("choices")]
        public IReadOnlyList<CommandOptionChoice> Choices { get; set; }

        [JsonPropertyName("options")]
        public IReadOnlyList<ApplicationCommandOption> Options { get; set; }
    }
}

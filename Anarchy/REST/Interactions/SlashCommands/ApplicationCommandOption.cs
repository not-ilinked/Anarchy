using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public class ApplicationCommandOption
    {
        [JsonProperty("type")]
        public CommandOptionType Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("required")]
        public bool Required { get; set; }

        [JsonProperty("choices")]
        public IReadOnlyList<CommandOptionChoice> Choices { get; set; }

        [JsonProperty("options")]
        public IReadOnlyList<ApplicationCommandOption> Options { get; set; }
    }
}

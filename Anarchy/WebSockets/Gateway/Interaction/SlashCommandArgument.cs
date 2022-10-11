using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class SlashCommandArgument
    {
        [JsonProperty("type")]
        public CommandOptionType Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public dynamic Value { get; set; }

        [JsonProperty("focused")]
        [JsonIgnore]
        public bool Focused { get; set; }

        [JsonProperty("options")]
        public List<SlashCommandArgument> Options { get; set; }
    }
}

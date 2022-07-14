using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class SlashCommandArgument
    {
        [JsonProperty("type")]
        public CommandOptionType Type { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("value")]
        public string Value { get; private set; }

        [JsonProperty("options")]
        public IReadOnlyList<SlashCommandArgument> Options { get; private set; }
    }
}

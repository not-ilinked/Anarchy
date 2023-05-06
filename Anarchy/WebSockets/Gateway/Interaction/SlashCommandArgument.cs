using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class SlashCommandArgument
    {
        [JsonPropertyName("type")]
        public CommandOptionType Type { get; private set; }

        [JsonPropertyName("name")]
        public string Name { get; private set; }

        [JsonPropertyName("value")]
        public string Value { get; private set; }

        [JsonPropertyName("options")]
        public IReadOnlyList<SlashCommandArgument> Options { get; private set; }
    }
}

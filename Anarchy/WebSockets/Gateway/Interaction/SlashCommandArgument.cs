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
    }
}

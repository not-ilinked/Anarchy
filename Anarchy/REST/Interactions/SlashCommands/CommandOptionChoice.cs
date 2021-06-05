using Newtonsoft.Json;

namespace Discord
{
    public class CommandOptionChoice
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }
    }
}

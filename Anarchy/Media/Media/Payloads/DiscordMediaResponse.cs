using Newtonsoft.Json;

namespace Discord.Media
{
    public class DiscordMediaResponse
    {
        [JsonProperty("op")]
        public DiscordMediaOpcode Opcode { get; private set; }


        [JsonProperty("d")]
        public object Data { get; private set; }
    }
}

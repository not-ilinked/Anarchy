using Discord.Gateway;
using Newtonsoft.Json;

namespace Discord.Media
{
    internal class DiscordMediaRequest<T>
    {
        [JsonProperty("op")]
        public DiscordMediaOpcode Opcode { get; set; }


        [JsonProperty("d")]
        public T Payload { get; set; }
    }
}

using Newtonsoft.Json;

namespace Discord.Voice
{
    public class DiscordVoiceRequest<T>
    {
        [JsonProperty("op")]
        public DiscordVoiceOpcode Opcode { get; set; }


        [JsonProperty("d")]
        public T Payload { get; set; }
    }
}

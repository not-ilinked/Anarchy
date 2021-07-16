using Newtonsoft.Json;

namespace Discord
{
    public class MessageInputComponent : MessageComponent
    {
        [JsonProperty("custom_id")]
        public string Id { get; set; }

        [JsonProperty("disabled")]
        public bool Disabled { get; set; }
    }
}

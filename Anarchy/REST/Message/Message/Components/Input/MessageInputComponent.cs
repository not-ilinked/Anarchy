

using System.Text.Json.Serialization;
namespace Discord
{
    public class MessageInputComponent : MessageComponent
    {
        [JsonPropertyName("custom_id")]
        public string Id { get; set; }

        [JsonPropertyName("disabled")]
        public bool Disabled { get; set; }
    }
}

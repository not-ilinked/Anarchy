
using System.Text.Json.Serialization;

namespace Discord
{
    public class MessageComponent
    {
        [JsonPropertyName("type")]
        public MessageComponentType Type { get; protected set; }
    }
}

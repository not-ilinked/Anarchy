

using System.Text.Json.Serialization;

namespace Discord
{
    public class ButtonComponent : MessageInputComponent
    {
        public ButtonComponent()
        {
            Type = MessageComponentType.Button;
        }

        [JsonPropertyName("style")]
        public MessageButtonStyle Style { get; set; }

        [JsonPropertyName("label")]
        public string Text { get; set; }

        [JsonPropertyName("emoji")]
        public PartialEmoji Emoji { get; set; }

        [JsonPropertyName("url")]
        public string RedirectUrl { get; set; }
    }
}

using Newtonsoft.Json;

namespace Discord
{
    public class ButtonComponent : MessageInputComponent
    {
        public ButtonComponent()
        {
            Type = MessageComponentType.Button;
        }

        [JsonProperty("style")]
        public MessageButtonStyle Style { get; set; }

        [JsonProperty("label")]
        public string Text { get; set; }

        [JsonProperty("emoji")]
        public PartialEmoji Emoji { get; set; }

        [JsonProperty("url")]
        public string RedirectUrl { get; set; }
    }
}

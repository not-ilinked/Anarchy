

using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class CustomStatusActivity : DiscordActivity
    {
        public CustomStatusActivity()
        {
            OnClientUpdated += (s, e) => Emoji.SetClient(Client);
        }

        [JsonPropertyName("state")]
        public string Text { get; private set; }

        [JsonPropertyName("emoji")]
        public PartialEmoji Emoji { get; private set; }

        public override string ToString()
        {
            return Text;
        }
    }
}

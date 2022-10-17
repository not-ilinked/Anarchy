using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class CustomStatusActivity : DiscordActivity
    {
        public CustomStatusActivity()
        {
            OnClientUpdated += (s, e) => Emoji.SetClient(Client);
        }

        [JsonProperty("state")]
        public string Text { get; private set; }

        [JsonProperty("emoji")]
        public PartialEmoji Emoji { get; private set; }

        public override string ToString()
        {
            return Text;
        }
    }
}

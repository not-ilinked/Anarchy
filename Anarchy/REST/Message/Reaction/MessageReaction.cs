

using System.Text.Json.Serialization;

namespace Discord
{
    public class MessageReaction : Controllable
    {
        public MessageReaction()
        {
            OnClientUpdated += (sender, e) => Emoji.SetClient(Client);
        }

        [JsonPropertyName("emoji")]
        public PartialEmoji Emoji { get; private set; }

        [JsonPropertyName("count")]
        public uint Count { get; private set; }

        [JsonPropertyName("me")]
        public bool ClientHasReacted { get; private set; }

        public override string ToString()
        {
            return Emoji.ToString();
        }
    }
}
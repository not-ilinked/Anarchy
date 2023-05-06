using System.Text.Json.Serialization;

namespace Discord
{
    public class WelcomeChannel : Controllable
    {
        [JsonPropertyName("channel_id")]
        private readonly ulong _id;

        public MinimalTextChannel Channel
        {
            get { return new MinimalTextChannel(_id).SetClient(Client); }
        }

        [JsonPropertyName("description")]
        public string Description { get; private set; }

        [JsonPropertyName("emoji_id")]
        public ulong? EmojiId { get; private set; }

        [JsonPropertyName("emoji_name")]
        public string EmojiName { get; private set; }
    }
}

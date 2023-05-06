

using System.Text.Json.Serialization;

namespace Discord
{
    public class CustomStatus
    {
        private readonly DiscordParameter<ulong?> EmojiProperty = new DiscordParameter<ulong?>();
        [JsonPropertyName("emoji_id")]
        public ulong? EmojiId
        {
            get { return EmojiProperty; }
            set { EmojiProperty.Value = value; }
        }

        public bool ShouldSerializeEmojiId()
        {
            return EmojiProperty.Set;
        }

        [JsonPropertyName("emoji_name")]
        public string EmojiName { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}

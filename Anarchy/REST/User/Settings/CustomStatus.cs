using Newtonsoft.Json;

namespace Discord
{
    public class CustomStatus
    {
        private readonly DiscordParameter<ulong?> EmojiProperty = new DiscordParameter<ulong?>();
        [JsonProperty("emoji_id")]
        public ulong? EmojiId
        {
            get => EmojiProperty;
            set => EmojiProperty.Value = value;
        }


        public bool ShouldSerializeEmojiId()
        {
            return EmojiProperty.Set;
        }


        [JsonProperty("emoji_name")]
        public string EmojiName { get; set; }


        [JsonProperty("text")]
        public string Text { get; set; }
    }
}

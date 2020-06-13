using Newtonsoft.Json;

namespace Discord
{
    public class CustomStatus
    {
        private readonly Property<ulong?> EmojiProperty = new Property<ulong?>();
        [JsonProperty("emoji_id")]
        public ulong? EmojiId
        {
            get { return EmojiProperty; }
            set { EmojiProperty.Value = value; }
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

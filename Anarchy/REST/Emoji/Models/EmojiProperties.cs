using System.Text.Json.Serialization;

namespace Discord
{
    /// <summary>
    /// Options for creating a <see cref="DiscordEmoji"/>
    /// </summary>
    public class EmojiProperties
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("image")]
        public DiscordImage Image { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
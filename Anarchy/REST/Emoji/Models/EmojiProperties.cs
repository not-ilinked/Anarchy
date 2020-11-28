using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Options for creating a <see cref="DiscordEmoji"/>
    /// </summary>
    public class EmojiProperties
    {
        [JsonProperty("name")]
        public string Name { get; set; }


        [JsonProperty("image")]
        public DiscordImage Image { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
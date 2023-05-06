using System.Text.Json.Serialization;

namespace Discord
{
    /// <summary>
    /// Options for creating a <see cref="DiscordChannel"/>
    /// </summary>
    public class ChannelCreationProperties
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public ChannelType Type { get; set; }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
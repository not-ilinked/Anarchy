

using System.Text.Json.Serialization;

namespace Discord
{
    /// <summary>
    /// Options for creating a <see cref="DiscordGuild"/>
    /// </summary>
    internal class GuildCreationProperties
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("region")]
        public string Region { get; set; }

        [JsonPropertyName("icon")]
        public DiscordImage Icon { get; set; }
    }
}
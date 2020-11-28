using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Options for creating a <see cref="DiscordGuild"/>
    /// </summary>
    internal class GuildCreationProperties
    {
        [JsonProperty("name")]
        public string Name { get; set; }


        [JsonProperty("region")]
        public string Region { get; set; }


        [JsonProperty("icon")]
        public DiscordImage Icon { get; set; }
    }
}
using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Options for creating a <see cref="DiscordChannel"/>
    /// </summary>
    public class ChannelCreationProperties
    {
        [JsonProperty("name")]
        public string Name { get; set; }


        [JsonProperty("type")]
        public ChannelType Type { get; set; }


        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Options for modifying a <see cref="DiscordChannel"/>    
    /// </summary>
    public class ChannelProperties
    {
        private readonly DiscordParameter<string> NameProperty = new DiscordParameter<string>();
        [JsonProperty("name")]
        public string Name
        {
            get { return NameProperty; }
            set { NameProperty.Value = value; }
        }


        public bool ShouldSerializeName()
        {
            return NameProperty.Set;
        }


        public override string ToString()
        {
            return Name;
        }
    }
}
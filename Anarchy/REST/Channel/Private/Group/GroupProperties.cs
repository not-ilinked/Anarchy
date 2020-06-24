using System.Drawing;
using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Options for modifying a <see cref="Group"/>
    /// </summary>
    public class GroupProperties : ChannelProperties
    {
        internal readonly DiscordParameter<DiscordImage> IconProperty = new DiscordParameter<DiscordImage>();
        [JsonProperty("icon")]
        public DiscordImage Icon
        {
            get { return IconProperty; }
            set { IconProperty.Value = value; }
        }


        public bool ShouldSerialize_icon()
        {
            return IconProperty.Set;
        }
    }
}

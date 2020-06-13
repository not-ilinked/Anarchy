using System.Drawing;
using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Options for modifying a <see cref="Group"/>
    /// </summary>
    public class GroupProperties : ChannelProperties
    {
        internal readonly Property<string> IconProperty = new Property<string>();
        [JsonProperty("icon")]
        private string _icon
        {
            get { return IconProperty; }
            set { IconProperty.Value = value; }
        }

        public Image Icon
        {
            get { return DiscordImage.ToImage(_icon); }
            set { _icon = DiscordImage.FromImage(value); }
        }


        public bool ShouldSerialize_icon()
        {
            return IconProperty.Set;
        }
    }
}

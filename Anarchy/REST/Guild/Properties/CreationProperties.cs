using Newtonsoft.Json;
using System.Drawing;

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
        private string _icon;

        public Image Icon
        {
            get { return DiscordImage.ToImage(_icon); }
            set { _icon = DiscordImage.FromImage(value); }
        }
    }
}
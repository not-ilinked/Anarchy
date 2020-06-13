using Newtonsoft.Json;
using System.Drawing;

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
        private string _image;

        
        public Image Image
        {
            get { return DiscordImage.ToImage(_image); }
            set { _image = DiscordImage.FromImage(value); }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json.Serialization;

namespace Discord
{
    public class DiscordEmbed
    {
        public DiscordEmbed()
        {
            Fields = new List<EmbedField>();
            Thumbnail = new EmbedImage();
            Image = new EmbedImage();
            Footer = new EmbedFooter();
            Author = new EmbedAuthor();
        }

        [JsonPropertyName("title")]
        public string Title { get; internal set; }

        [JsonPropertyName("url")]
        public string TitleUrl { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; internal set; }

        [JsonPropertyName("color")]
        private uint _color;
        public Color Color
        {
            get { return Color.FromArgb((int) _color); }
            set { _color = (uint) Color.FromArgb(0, value.R, value.G, value.B).ToArgb(); }
        }

        [JsonPropertyName("fields")]
        public IReadOnlyList<EmbedField> Fields { get; internal set; }

        [JsonPropertyName("video")]
        public EmbedVideo Video { get; private set; }

        [JsonPropertyName("thumbnail")]
        public EmbedImage Thumbnail { get; private set; }

        [JsonPropertyName("image")]
        public EmbedImage Image { get; private set; }

        [JsonPropertyName("footer")]
        public EmbedFooter Footer { get; internal set; }

        [JsonPropertyName("author")]
        public EmbedAuthor Author { get; internal set; }

        [JsonPropertyName("timestamp")]
        public DateTime? Timestamp { get; internal set; }

        public override string ToString()
        {
            return Title;
        }
    }
}

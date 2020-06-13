using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Discord
{
    /// <summary>
    /// Used to make <see cref="DiscordEmbed"/>s
    /// </summary>
    public class EmbedMaker
    {
        private readonly DiscordEmbed _embed;


        public string Title
        {
            get { return _embed.Title; }
            set
            {
                if (value.Length > 256)
                    throw new EmbedException(EmbedError.TitleTooLong);

                _embed.Title = value;
            }
        }


        public string TitleUrl
        {
            get { return _embed.TitleUrl; }
            set { _embed.TitleUrl = value; }
        }


        public string Description
        {
            get { return _embed.Description; }
            set
            {
                if (value.Length > 2048)
                    throw new EmbedException(EmbedError.DescriptionTooLong);

                _embed.Description = value;
            }
        }


        public Color Color
        {
            get { return _embed.Color; }
            set { _embed.Color = value; }
        }


        public EmbedMaker AddField(string name, string content, bool inline = false)
        {
            if (_embed.Fields.Count == 25)
                throw new EmbedException(EmbedError.TooManyFields);
            if (name.Length > 256)
                throw new EmbedException(EmbedError.FieldNameTooLong);
            if (content.Length > 1024)
                throw new EmbedException(EmbedError.FieldContentTooLong);


            List<EmbedField> fields = _embed.Fields.ToList();
            fields.Add(new EmbedField(name, content, inline));
            _embed.Fields = fields;

            return this;
        }


        public string ThumbnailUrl
        {
            get { return _embed.Thumbnail.Url; }
            set { _embed.Thumbnail.Url = value; }
        }


        public string ImageUrl
        {
            get { return _embed.Image.Url; }
            set { _embed.Image.Url = value; }
        }


        public EmbedFooter Footer
        {
            get { return _embed.Footer; }
        }


        public EmbedAuthor Author
        {
            get { return _embed.Author; }
        }


        public EmbedMaker()
        {
            _embed = new DiscordEmbed();
        }


        public static implicit operator DiscordEmbed(EmbedMaker instance)
        {
            return instance._embed;
        }



        public override string ToString()
        {
            return _embed.ToString();
        }
    }
}

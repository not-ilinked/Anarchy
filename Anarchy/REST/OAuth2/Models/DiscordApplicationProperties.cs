using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;

namespace Discord
{
    public class DiscordApplicationProperties
    {
        private readonly Property<string> _nameProperty = new Property<string>();
        [JsonProperty("name")]
        public string Name
        {
            get { return _nameProperty; }
            set { _nameProperty.Value = value; }
        }


        public bool ShouldSerializeName()
        {
            return _nameProperty.Set;
        }


        private readonly Property<string> _descriptionProperty = new Property<string>();
        [JsonProperty("description")]
        public string Description
        {
            get { return _descriptionProperty; }
            set { _descriptionProperty.Value = value; }
        }


        public bool ShouldSerializeDescription()
        {
            return _descriptionProperty.Set;
        }


        private readonly Property<string> IconProperty = new Property<string>();
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


        private readonly Property<bool> _publicProperty = new Property<bool>();
        [JsonProperty("bot_public")]
        public bool PublicBot
        {
            get { return _publicProperty; }
            set { _publicProperty.Value = value; }
        }


        public bool ShouldSerializePublicBot()
        {
            return _publicProperty.Set;
        }


        private readonly Property<bool> _codeGrantProperty = new Property<bool>();
        [JsonProperty("bot_require_code_grant")]
        public bool BotRequireCodeGrant
        {
            get { return _codeGrantProperty; }
            set { _codeGrantProperty.Value = value; }
        }


        public bool ShouldSerializeBotRequireCodeGrant()
        {
            return _codeGrantProperty.Set;
        }


        private readonly Property<List<string>> _redirectsProperty = new Property<List<string>>();
        [JsonProperty("redirect_urls")]
        public List<string> RedirectUrls
        {
            get { return _redirectsProperty; }
            set { _redirectsProperty.Value = value; }
        }


        public bool ShouldSerializeRedirectUrls()
        {
            return _redirectsProperty.Set;
        }
    }
}

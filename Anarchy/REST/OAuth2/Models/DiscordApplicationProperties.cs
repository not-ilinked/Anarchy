using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;

namespace Discord
{
    public class DiscordApplicationProperties
    {
        private readonly DiscordParameter<string> _nameProperty = new DiscordParameter<string>();
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


        private readonly DiscordParameter<string> _descriptionProperty = new DiscordParameter<string>();
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


        private readonly DiscordParameter<DiscordImage> IconProperty = new DiscordParameter<DiscordImage>();
        [JsonProperty("icon")]
        public DiscordImage Icon
        {
            get { return IconProperty; }
            set { IconProperty.Value = value; }
        }

        public bool ShouldSerializeIcon()
        {
            return IconProperty.Set;
        }


        private readonly DiscordParameter<bool> _publicProperty = new DiscordParameter<bool>();
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


        private readonly DiscordParameter<bool> _codeGrantProperty = new DiscordParameter<bool>();
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


        private readonly DiscordParameter<List<string>> _redirectsProperty = new DiscordParameter<List<string>>();
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

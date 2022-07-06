using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Options for sending a message through a webhook
    /// </summary>
    internal class WebhookMessageProperties
    {
        [JsonProperty("content")]
        public string Content { get; set; }


        [JsonProperty("embeds")]
        private List<DiscordEmbed> _embeds;
        public DiscordEmbed Embed
        {
            get
            {
                return _embeds == null || _embeds.Count == 0 ? null : _embeds[0];
            }
            set
            {
                if (value == null)
                    _embeds = null;
                else
                    _embeds = new List<DiscordEmbed>() { value };
            }
        }


        internal DiscordParameter<string> NameProperty = new DiscordParameter<string>();
        [JsonProperty("username")]
        public string Username
        {
            get { return NameProperty; }
            set { NameProperty.Value = value; }
        }


        public bool ShouldSerializeUsername()
        {
            return NameProperty.Set;
        }


        internal DiscordParameter<string> AvatarProperty = new DiscordParameter<string>();
        [JsonProperty("avatar_url")]
        public string AvatarUrl
        {
            get { return AvatarProperty; }
            set { AvatarProperty.Value = value; }
        }


        public bool ShouldSerializeAvatarUrl()
        {
            return AvatarProperty.Set;
        }
    }
}
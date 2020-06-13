using Newtonsoft.Json;
using System.Drawing;

namespace Discord.Webhook
{
    /// <summary>
    /// Options for creating/modifying a webhook
    /// </summary>
    public class DiscordWebhookProperties
    {
        private readonly Property<string> NameProperty = new Property<string>();
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


        private readonly Property<string> AvatarProperty = new Property<string>();
        [JsonProperty("avatar")]
        private string _avatar
        {
            get { return AvatarProperty; }
            set { AvatarProperty.Value = value; }
        }

        public Image Avatar
        {
            get { return DiscordImage.ToImage(_avatar); }
            set { _avatar = DiscordImage.FromImage(value); }
        }


        public bool ShouldSeriaize_avatar()
        {
            return AvatarProperty.Set;
        }


        private readonly Property<ulong> ChannelProperty = new Property<ulong>();
        [JsonProperty("channel_id")]
        public ulong ChannelId
        {
            get { return ChannelProperty; }
            set { ChannelProperty.Value = value; }
        }


        public bool ShouldSerializeChannelId()
        {
            return ChannelProperty.Set;
        }


        public override string ToString()
        {
            return Name;
        }
    }
}

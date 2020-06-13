using Newtonsoft.Json;
using System.Drawing;

namespace Discord
{
    /// <summary>
    /// Options for modifying a <see cref="DiscordGuild"/>
    /// </summary>
    public class GuildProperties
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


        private readonly Property<string> RegionProperty = new Property<string>();
        [JsonProperty("region")]
        public string Region
        {
            get { return RegionProperty; }
            set { RegionProperty.Value = value; }
        }


        public bool ShouldSerializeRegion()
        {
            return RegionProperty.Set;
        }


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


        private readonly Property<ulong> OwnerProperty = new Property<ulong>();
        [JsonProperty("owner_id")]
        public ulong OwnerId
        {
            get { return OwnerProperty; }
            set { OwnerProperty.Value = value; }
        }

        
        public bool ShouldSerializeOwnerId()
        {
            return OwnerProperty.Set;
        }


        private readonly Property<GuildVerificationLevel> VerificationProperty = new Property<GuildVerificationLevel>();
        [JsonProperty("verification_level")]
        public GuildVerificationLevel VerificationLevel
        {
            get { return VerificationProperty; }
            set { VerificationProperty.Value = value; }
        }


        public bool ShouldSerializeVerificationLevel()
        {
            return VerificationProperty.Set;
        }


        private readonly Property<GuildDefaultNotifications> NotificationsProperty = new Property<GuildDefaultNotifications>();
        [JsonProperty("default_message_notifications")]
        public GuildDefaultNotifications DefaultNotifications
        {
            get { return NotificationsProperty; }
            set { NotificationsProperty.Value = value; }
        }


        public bool ShouldSerializeDefaultNotifications()
        {
            return NotificationsProperty.Set;
        }


        internal readonly Property<string> VanityProperty = new Property<string>();
        public string VanityUrlCode
        {
            get { return VanityProperty; }
            set { VanityProperty.Value = value; }
        }
    }
}

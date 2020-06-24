using Newtonsoft.Json;
using System.Drawing;

namespace Discord
{
    /// <summary>
    /// Options for modifying a <see cref="DiscordGuild"/>
    /// </summary>
    public class GuildProperties
    {
        private readonly DiscordParameter<string> NameProperty = new DiscordParameter<string>();
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


        private readonly DiscordParameter<string> RegionProperty = new DiscordParameter<string>();
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


        internal readonly DiscordParameter<DiscordImage> IconProperty = new DiscordParameter<DiscordImage>();
        [JsonProperty("icon")]
        public DiscordImage Icon
        {
            get { return IconProperty; }
            set { IconProperty.Value = value; }
        }


        public bool ShouldSerialize_icon()
        {
            return IconProperty.Set;
        }


        private readonly DiscordParameter<ulong> OwnerProperty = new DiscordParameter<ulong>();
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


        private readonly DiscordParameter<GuildVerificationLevel> VerificationProperty = new DiscordParameter<GuildVerificationLevel>();
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


        private readonly DiscordParameter<GuildDefaultNotifications> NotificationsProperty = new DiscordParameter<GuildDefaultNotifications>();
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


        internal readonly DiscordParameter<string> VanityProperty = new DiscordParameter<string>();
        public string VanityUrlCode
        {
            get { return VanityProperty; }
            set { VanityProperty.Value = value; }
        }
    }
}

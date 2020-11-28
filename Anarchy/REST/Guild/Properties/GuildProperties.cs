using Newtonsoft.Json;
using System.Collections.Generic;
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


        public bool ShouldSerializeIcon()
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


        private readonly DiscordParameter<List<string>> FeatureProperty = new DiscordParameter<List<string>>();
        [JsonProperty("features")]
        public List<string> Features
        {
            get { return FeatureProperty; }
            set { FeatureProperty.Value = value; }
        }

        public bool ShouldSerializeFeatures()
        {
            return FeatureProperty.Set;
        }


        private readonly DiscordParameter<ulong?> _updatesChannel = new DiscordParameter<ulong?>();
        [JsonProperty("public_updates_channel_id")]
        public ulong? PublicUpdatesChannelId
        {
            get { return _updatesChannel; }
            set { _updatesChannel.Value = value; }
        }

        public bool ShouldSerializePublicUpdatesChannelId()
        {
            return _updatesChannel.Set;
        }


        private readonly DiscordParameter<ulong?> _rulesChannel = new DiscordParameter<ulong?>();
        [JsonProperty("rules_channel_id")]
        public ulong? RulesChannelId
        {
            get { return _rulesChannel; }
            set { _rulesChannel.Value = value; }
        }

        public bool ShouldSerializeRulesChannelId()
        {
            return _rulesChannel.Set;
        }


        private readonly DiscordParameter<DiscordLanguage?> _preferredLocale = new DiscordParameter<DiscordLanguage?>();
        [JsonProperty("preferred_locale")]
        public DiscordLanguage? PreferredLanguage
        {
            get { return _preferredLocale; }
            set { _preferredLocale.Value = value; }
        }

        public bool ShouldSerializePreferredLanguage()
        {
            return _preferredLocale.Set;
        }


        internal readonly DiscordParameter<string> VanityProperty = new DiscordParameter<string>();
        public string VanityUrlCode
        {
            get { return VanityProperty; }
            set { VanityProperty.Value = value; }
        }
    }
}

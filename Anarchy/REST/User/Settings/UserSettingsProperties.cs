using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Discord
{
    public class UserSettingsProperties
    {
        private readonly DiscordParameter<string> ThemeProperty = new DiscordParameter<string>();
        [JsonProperty("theme")]
        private string _theme
        {
            get { return ThemeProperty.Value; }
        }

        public DiscordTheme Theme
        {
            get { return (DiscordTheme)Enum.Parse(typeof(DiscordTheme), _theme, true); }
            set { ThemeProperty.Value = value.ToString().ToLower(); }
        }


        public bool ShouldSerializeTheme()
        {
            return ThemeProperty.Set;
        }


        private readonly DiscordParameter<ExplicitContentFilter> ExplicityProperty = new DiscordParameter<ExplicitContentFilter>();
        [JsonProperty("explicit_content_filter")]
        public ExplicitContentFilter ExplicitContentFilter
        {
            get { return ExplicityProperty; }
            set { ExplicityProperty.Value = value; }
        }


        public bool ShouldSerializeExplicitContentFilter()
        {
            return ExplicityProperty.Set;
        }


        private readonly DiscordParameter<bool> DevProperty = new DiscordParameter<bool>();
        [JsonProperty("developer_mode")]
        public bool DeveloperMode
        {
            get { return DevProperty; }
            set { DevProperty.Value = value; }
        }


        public bool ShouldSerializeDeveloperMode()
        {
            return DevProperty.Set;
        }


        private readonly DiscordParameter<bool> CompactProperty = new DiscordParameter<bool>();
        [JsonProperty("message_display_compact")]
        public bool CompactMessages
        {
            get { return CompactProperty; }
            set { CompactProperty.Value = value; }
        }


        public bool ShouldSerializeCompactMessages()
        {
            return CompactProperty.Set;
        }


        private readonly DiscordParameter<DiscordLanguage> LocaleProperty = new DiscordParameter<DiscordLanguage>();
        [JsonProperty("locale")]
#pragma warning disable IDE0051
        private string _locale
        {
            get { return LanguageUtils.LanguageToString(LocaleProperty); }
        }
#pragma warning restore IDE0051

        public DiscordLanguage Language
        {
            get { return LocaleProperty.Value; }
            set { LocaleProperty.Value = value; }
        }


        public bool ShouldSerialize_locale()
        {
            return LocaleProperty.Set;
        }


        private readonly DiscordParameter<bool> TtsProperty = new DiscordParameter<bool>();
        [JsonProperty("enable_tts_playback")]
        public bool EnableTts
        {
            get { return TtsProperty; }
            set { TtsProperty.Value = value; }
        }


        public bool ShouldSerializeEnableTts()
        {
            return TtsProperty.Set;
        }


        private readonly DiscordParameter<bool> GifProperty = new DiscordParameter<bool>();
        [JsonProperty("gif_auto_play")]
        public bool PlayGifsAutomatically
        {
            get { return GifProperty; }
            set { GifProperty.Value = value; }
        }


        public bool ShouldSerializePlayGifsAutomatically()
        {
            return GifProperty.Set;
        }


        private readonly DiscordParameter<CustomStatus> StatusProperty = new DiscordParameter<CustomStatus>();
        [JsonProperty("custom_status")]
        public CustomStatus CustomStatus
        {
            get { return StatusProperty; }
            set { StatusProperty.Value = value; }
        }


        public bool ShouldSerializeCustomStatus()
        {
            return StatusProperty.Set;
        }


        private readonly DiscordParameter<List<DiscordGuildFolderUpdate>> _folderProperty = new DiscordParameter<List<DiscordGuildFolderUpdate>>();
        [JsonProperty("guild_folders")]
        public List<DiscordGuildFolderUpdate> GuildFolders
        {
            get { return _folderProperty; }
            set { _folderProperty.Value = value; }
        }
    }
}

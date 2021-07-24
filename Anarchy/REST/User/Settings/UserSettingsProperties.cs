using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Discord
{
    public class UserSettingsProperties
    {
        // Privacy & Safety
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


        private readonly DiscordParameter<bool> _defaultGuildRestrictParam = new DiscordParameter<bool>();
        [JsonProperty("default_guilds_restricted")]
        public bool RestrictGuildsByDefault
        {
            get { return _defaultGuildRestrictParam; }
            set { _defaultGuildRestrictParam.Value = value; }
        }

        public bool ShouldSerializeRestrictGuildsByDefault() => _defaultGuildRestrictParam.Set;


        private readonly DiscordParameter<bool> _nsfwGuildParam = new DiscordParameter<bool>();
        [JsonProperty("view_nsfw_guilds")]
        public bool ViewNsfwGuilds
        {
            get { return _nsfwGuildParam; }
            set { _nsfwGuildParam.Value = value; }
        }

        public bool ShouldSerializeViewNsfwGuilds() => _nsfwGuildParam.Set;


        private readonly DiscordParameter<FriendRequestFlags> _friendFlags = new DiscordParameter<FriendRequestFlags>();
        [JsonProperty("friend_source_flags")]
        public FriendRequestFlags FriendRequestFlags
        {
            get { return _friendFlags; }
            set { _friendFlags.Value = value; }
        }

        public bool ShouldSerializeFriendRequestFlags() => _friendFlags.Set;


        // Appearance
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

        public bool ShouldSerialize_theme()
        {
            return ThemeProperty.Set;
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


        // Accessability
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


        private readonly DiscordParameter<bool> _emojiProperty = new DiscordParameter<bool>();
        [JsonProperty("animate_emoji")]
        public bool PlayAnimatedEmojis
        {
            get { return _emojiProperty; }
            set { _emojiProperty.Value = value; }
        }

        public bool ShouldSerializePlayAnimatedEmojis() => _emojiProperty.Set;


        private readonly DiscordParameter<StickerAnimationAvailability> _stickerProperty = new DiscordParameter<StickerAnimationAvailability>();
        [JsonProperty("animate_stickers")]
        public StickerAnimationAvailability StickerAnimation
        {
            get { return _stickerProperty; }
            set { _stickerProperty.Value = value; }
        }

        public bool ShouldSerializeStickerAnimation() => _stickerProperty.Set;


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


        // Text and images
        private readonly DiscordParameter<bool> _mediaParam = new DiscordParameter<bool>();
        [JsonProperty("inline_embed_media")]
        public bool EmbedMedia
        {
            get { return _mediaParam; }
            set { _mediaParam.Value = value; }
        }

        public bool ShouldSerializeEmbedMedia() => _mediaParam.Set;


        private readonly DiscordParameter<bool> _attachmentParam = new DiscordParameter<bool>();
        [JsonProperty("inline_attachment_media")]
        public bool EmbedAttachments
        {
            get { return _attachmentParam; }
            set { _attachmentParam.Value = value; }
        }

        public bool ShouldSerializeEmbedAttachments() => _attachmentParam.Set;


        private readonly DiscordParameter<bool> _embedParam = new DiscordParameter<bool>();
        [JsonProperty("render_embeds")]
        public bool EmbedLinks
        {
            get { return _embedParam; }
            set { _embedParam.Value = value; }
        }

        public bool ShouldSerializeEmbedLinks() => _embedParam.Set;


        private readonly DiscordParameter<bool> _reactionParam = new DiscordParameter<bool>();
        [JsonProperty("render_reactions")]
        public bool ShowReactions
        {
            get { return _reactionParam; }
            set { _reactionParam.Value = value; }
        }

        public bool ShouldSerializeShowReactions() => _reactionParam.Set;


        private readonly DiscordParameter<bool> _emoticonParam = new DiscordParameter<bool>();
        [JsonProperty("convert_emoticons")]
        public bool ConvertEmoticons
        {
            get { return _emoticonParam; }
            set { _emoticonParam.Value = value; }
        }

        public bool ShouldSerializeConvertEmoticons() => _emoticonParam.Set;


        // Language
        private readonly DiscordParameter<DiscordLanguage> LocaleProperty = new DiscordParameter<DiscordLanguage>();
        [JsonProperty("locale")]
        public DiscordLanguage Language
        {
            get { return LocaleProperty.Value; }
            set { LocaleProperty.Value = value; }
        }


        public bool ShouldSerializeLanguage()
        {
            return LocaleProperty.Set;
        }


        // Advanced
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


        // Other
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


        private readonly DiscordParameter<List<ulong>> _guildRestrictParam = new DiscordParameter<List<ulong>>();
        [JsonProperty("restricted_guilds")]
        public List<ulong> RestrictedGuilds
        {
            get { return _guildRestrictParam; }
            set { _guildRestrictParam.Value = value; }
        }

        public bool ShouldSerializeRestrictedGuilds() => _guildRestrictParam.Set;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord
{
    public class DiscordUserSettings : Controllable
    {
        public DiscordUserSettings()
        {
            GuildFolders.SetClientsInList(Client);
        }

        // Privacy & Safety
        [JsonPropertyName("explicit_content_filter")]
        public ExplicitContentFilter ExplicitContentFilter { get; private set; }

        [JsonPropertyName("default_guilds_restricted")]
        public bool RestrictGuildsByDefault { get; private set; }

        [JsonPropertyName("view_nsfw_guilds")]
        public bool ViewNsfwGuilds { get; private set; }

        [JsonPropertyName("friend_source_flags")]
        public FriendRequestFlags FriendRequestFlags { get; private set; }

        // Appearance
        [JsonPropertyName("theme")]
        private string _theme;

        public DiscordTheme Theme => (DiscordTheme) Enum.Parse(typeof(DiscordTheme), _theme, true);

        [JsonPropertyName("message_display_compact")]
        public bool CompactMessages { get; private set; }

        // Accessability
        [JsonPropertyName("gif_auto_play")]
        public bool PlayGifsAutomatically { get; private set; }

        [JsonPropertyName("animate_emoji")]
        public bool PlayAnimatedEmojis { get; private set; }

        [JsonPropertyName("animate_stickers")]
        public StickerAnimationAvailability StickerAnimation { get; private set; }

        [JsonPropertyName("enable_tts_playback")]
        public bool EnableTts { get; private set; }

        // Text and images
        [JsonPropertyName("inline_embed_media")]
        public bool EmbedMedia { get; private set; }

        [JsonPropertyName("inline_attachment_media")]
        public bool EmbedAttachments { get; private set; }

        [JsonPropertyName("render_embeds")]
        public bool EmbedLinks { get; private set; }

        [JsonPropertyName("render_reactions")]
        public bool ShowReactions { get; private set; }

        [JsonPropertyName("convert_emoticons")]
        public bool ConvertEmoticons { get; private set; }

        // Language
        [JsonPropertyName("locale")]
        public DiscordLanguage Language { get; private set; }

        // Advanced
        [JsonPropertyName("developer_mode")]
        public bool DeveloperMode { get; private set; }

        // Other
        [JsonPropertyName("custom_status")]
        public CustomStatus CustomStatus { get; private set; }

        [JsonPropertyName("guild_positions")]
        public IReadOnlyList<ulong> GuildPositions { get; private set; }

        [JsonPropertyName("guild_folders")]
        public IReadOnlyList<DiscordGuildFolder> GuildFolders { get; private set; }

        [JsonPropertyName("restricted_guilds")]
        private readonly List<ulong> _restrictedGuilds;
        public IReadOnlyList<MinimalGuild> RestrictedGuilds => _restrictedGuilds.Select(id => new MinimalGuild(id).SetClient(Client)).ToList();

        internal void Update(JsonElement element)
        {
            foreach (var property in this.GetType().GetProperties())
            {
                foreach (var attr in property.GetCustomAttributes(false))
                {
                    if (attr.GetType() == typeof(JsonPropertyNameAttribute))
                    {
                        var jsonAttr = (JsonPropertyNameAttribute) attr;

                        if (element.TryGetProperty(jsonAttr.Name, out JsonElement value))
                            property.SetValue(this, JsonSerializer.Deserialize(value.GetRawText(), property.PropertyType));

                        break;
                    }
                }
            }

            if (element.TryGetProperty("theme", out JsonElement theme))
                _theme = theme.GetString();
        }
    }
}

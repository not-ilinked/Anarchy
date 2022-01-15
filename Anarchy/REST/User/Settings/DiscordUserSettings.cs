using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Discord
{
    public class DiscordUserSettings : Controllable
    {
        public DiscordUserSettings()
        {
            GuildFolders.SetClientsInList(Client);
        }


        // Privacy & Safety
        [JsonProperty("explicit_content_filter")]
        public ExplicitContentFilter ExplicitContentFilter { get; private set; }

        [JsonProperty("default_guilds_restricted")]
        public bool RestrictGuildsByDefault { get; private set; }

        [JsonProperty("view_nsfw_guilds")]
        public bool ViewNsfwGuilds { get; private set; }

        [JsonProperty("friend_source_flags")]
        public FriendRequestFlags FriendRequestFlags { get; private set; }


        // Appearance
        [JsonProperty("theme")]
        private string _theme;

        public DiscordTheme Theme => (DiscordTheme)Enum.Parse(typeof(DiscordTheme), _theme, true);

        [JsonProperty("message_display_compact")]
        public bool CompactMessages { get; private set; }


        // Accessability
        [JsonProperty("gif_auto_play")]
        public bool PlayGifsAutomatically { get; private set; }

        [JsonProperty("animate_emoji")]
        public bool PlayAnimatedEmojis { get; private set; }

        [JsonProperty("animate_stickers")]
        public StickerAnimationAvailability StickerAnimation { get; private set; }

        [JsonProperty("enable_tts_playback")]
        public bool EnableTts { get; private set; }


        // Text and images
        [JsonProperty("inline_embed_media")]
        public bool EmbedMedia { get; private set; }

        [JsonProperty("inline_attachment_media")]
        public bool EmbedAttachments { get; private set; }

        [JsonProperty("render_embeds")]
        public bool EmbedLinks { get; private set; }

        [JsonProperty("render_reactions")]
        public bool ShowReactions { get; private set; }

        [JsonProperty("convert_emoticons")]
        public bool ConvertEmoticons { get; private set; }


        // Language
        [JsonProperty("locale")]
        public DiscordLanguage Language { get; private set; }


        // Advanced
        [JsonProperty("developer_mode")]
        public bool DeveloperMode { get; private set; }


        // Other
        [JsonProperty("custom_status")]
        public CustomStatus CustomStatus { get; private set; }

        [JsonProperty("guild_positions")]
        public IReadOnlyList<ulong> GuildPositions { get; private set; }

        [JsonProperty("guild_folders")]
        public IReadOnlyList<DiscordGuildFolder> GuildFolders { get; private set; }

        [JsonProperty("restricted_guilds")]
        private readonly List<ulong> _restrictedGuilds;
        public IReadOnlyList<MinimalGuild> RestrictedGuilds => _restrictedGuilds.Select(id => new MinimalGuild(id).SetClient(Client)).ToList();


        internal void Update(JObject jObj)
        {
            foreach (System.Reflection.PropertyInfo property in GetType().GetProperties())
            {
                foreach (object attr in property.GetCustomAttributes(false))
                {
                    if (attr.GetType() == typeof(JsonPropertyAttribute))
                    {
                        JsonPropertyAttribute jsonAttr = (JsonPropertyAttribute)attr;

                        if (jObj.TryGetValue(jsonAttr.PropertyName, out JToken value))
                        {
                            property.SetValue(this, value.ToObject(property.PropertyType));
                        }

                        break;
                    }
                }
            }

            if (jObj.TryGetValue("theme", out JToken theme))
            {
                _theme = theme.ToObject<string>();
            }
        }
    }
}

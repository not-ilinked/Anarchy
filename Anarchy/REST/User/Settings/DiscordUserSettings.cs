using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Discord
{
    public class DiscordUserSettings
    {
        [JsonProperty("theme")]
        private readonly string _theme;

        public DiscordTheme Theme
        {
            get { return (DiscordTheme)Enum.Parse(typeof(DiscordTheme), _theme, true); }
        }


        [JsonProperty("explicit_content_filter")]
        public ExplicitContentFilter ExplicitContentFilter { get; private set; }


        [JsonProperty("developer_mode")]
        public bool DeveloperMode { get; private set; }


        [JsonProperty("message_display_compact")]
        public bool CompactMessages { get; private set; }


        [JsonProperty("locale")]
        private readonly string _locale;

        public DiscordLanguage Language
        {
            get { return LanguageUtils.StringToLanguage(_locale); }
        }


        [JsonProperty("enable_tts_playback")]
        public bool EnableTts { get; private set; }


        [JsonProperty("gif_auto_play")]
        public bool PlayGifsAutomatically { get; private set; }


        [JsonProperty("custom_status")]
        public CustomStatus CustomStatus { get; private set; }


        [JsonProperty("guild_positions")]
        private IReadOnlyList<ulong> _guildPositions;

        public IReadOnlyList<MinimalGuild> GuildPositions
        {
            get
            {
                List<MinimalGuild> guilds = new List<MinimalGuild>();
                foreach (var guildId in _guildPositions)
                    guilds.Add(new MinimalGuild(guildId));

                return guilds;
            }
        }


        [JsonProperty("guild_folders")]
        public IReadOnlyList<DiscordGuildFolder> GuildFolders { get; private set; }
    }
}

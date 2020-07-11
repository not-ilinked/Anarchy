using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Discord
{
    public class DiscordUserSettings : Controllable
    {
        public DiscordUserSettings()
        {
            GuildFolders.SetClientsInList(Client);
        }

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
        public DiscordLanguage Language { get; private set; }


        [JsonProperty("enable_tts_playback")]
        public bool EnableTts { get; private set; }


        [JsonProperty("gif_auto_play")]
        public bool PlayGifsAutomatically { get; private set; }


        [JsonProperty("custom_status")]
        public CustomStatus CustomStatus { get; private set; }


        [JsonProperty("guild_positions")]
        public IReadOnlyList<ulong> GuildPositions { get; private set; }


        [JsonProperty("guild_folders")]
        public IReadOnlyList<DiscordGuildFolder> GuildFolders { get; private set; }
    }
}

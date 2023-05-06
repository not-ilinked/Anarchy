using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json.Serialization;

namespace Discord
{
    public class DiscordGuildFolderUpdate
    {
        private readonly Random _rnd;

        public DiscordGuildFolderUpdate()
        {
            _rnd = new Random();
        }

        [JsonPropertyName("guild_ids")]
        public List<ulong> Guilds { get; set; }

        [JsonPropertyName("id")]
        public long? Id { get; internal set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("color")]
        private int? _color;

        public Color? Color
        {
            get
            {
                if (_color.HasValue)
                    return System.Drawing.Color.FromArgb(_color.Value);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    Color val = value.Value;

                    _color = System.Drawing.Color.FromArgb(0, val.R, val.G, val.B).ToArgb();
                }
                else
                    _color = null;
            }
        }

        public bool Folder
        {
            get { return Id.HasValue; }
            set
            {
                if (value)
                {
                    if (!Id.HasValue)
                        Id = (long) _rnd.Next(1, 999999);
                }
                else
                    Id = null;
            }
        }

        public static List<DiscordGuildFolderUpdate> FromSettings(DiscordUserSettings settings)
        {
            return settings.GuildFolders.ToList().ConvertAll(f => f.ToUpdate());
        }
    }
}

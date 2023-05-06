using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json.Serialization;

namespace Discord
{
    public class DiscordGuildFolder : Controllable
    {
        [JsonPropertyName("guild_ids")]
        private readonly IReadOnlyList<ulong> _guilds;

        public IReadOnlyList<MinimalGuild> Guilds
        {
            get
            {
                List<MinimalGuild> guilds = new List<MinimalGuild>();
                foreach (var guildId in _guilds)
                    guilds.Add(new MinimalGuild(guildId).SetClient(Client));

                return guilds;
            }
        }

        [JsonPropertyName("id")]
        public long? Id { get; private set; }

        [JsonPropertyName("name")]
        public string Name { get; private set; }

        [JsonPropertyName("color")]
        private readonly int? _color;

        public Color? Color
        {
            get
            {
                if (_color.HasValue)
                    return System.Drawing.Color.FromArgb(_color.Value);
                else
                    return null;
            }
        }

        public bool Folder
        {
            get { return Id.HasValue; }
        }

        public DiscordGuildFolderUpdate ToUpdate()
        {
            return new DiscordGuildFolderUpdate()
            {
                Guilds = _guilds.ToList(),
                Id = Id,
                Name = Name,
                Color = Color
            };
        }
    }
}

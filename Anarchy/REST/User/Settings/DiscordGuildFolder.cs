using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;

namespace Discord
{
    public class DiscordGuildFolder : Controllable
    {
        [JsonProperty("guild_ids")]
        private IReadOnlyList<ulong> _guilds;


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


        [JsonProperty("id")]
        public long? Id { get; private set; }


        [JsonProperty("name")]
        public string Name { get; private set; }


        [JsonProperty("color")]
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

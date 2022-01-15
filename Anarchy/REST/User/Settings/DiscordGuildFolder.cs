﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Discord
{
    public class DiscordGuildFolder : Controllable
    {
        [JsonProperty("guild_ids")]
        private readonly IReadOnlyList<ulong> _guilds;


        public IReadOnlyList<MinimalGuild> Guilds
        {
            get
            {
                List<MinimalGuild> guilds = new List<MinimalGuild>();
                foreach (ulong guildId in _guilds)
                {
                    guilds.Add(new MinimalGuild(guildId).SetClient(Client));
                }

                return guilds;
            }
        }


        [JsonProperty("id")]
        public long? Id { get; private set; }


        [JsonProperty("name")]
        public string Name { get; private set; }


        [JsonProperty("color")]
        private readonly int? _color;

        public Color? Color
        {
            get
            {
                if (_color.HasValue)
                {
                    return System.Drawing.Color.FromArgb(_color.Value);
                }
                else
                {
                    return null;
                }
            }
        }


        public bool Folder => Id.HasValue;


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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Discord
{
    public class PartialGuildMember : Controllable, IDisposable
    {
        [JsonProperty("guild_id")]
        internal ulong GuildId { get; set; }


        public MinimalGuild Guild
        {
            get
            {
                return new MinimalGuild(GuildId).SetClient(Client);
            }
        }


        [JsonProperty("nick")]
        public string Nickname { get; internal set; }


        [JsonProperty("roles")]
        protected List<ulong> _roles;

        public IReadOnlyList<MinimalRole> Roles
        {
            get
            {
                var roles = new List<MinimalRole>();

                foreach (var role in _roles)
                    roles.Add(new MinimalRole(GuildId, role).SetClient(Client));

                return roles;
            }
        }


        [JsonProperty("joined_at")]
        public DateTime? JoinedAt { get; private set; }

        [JsonProperty("premium_since")]
        public DateTime? BoostingSince { get; private set; }


        [JsonProperty("mute")]
        public bool Muted { get; protected set; }


        [JsonProperty("deaf")]
        public bool Deafened { get; protected set; }


        public new void Dispose()
        {
            base.Dispose();
            Nickname = null;
            _roles = null;
        }
    }
}

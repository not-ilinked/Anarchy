using Discord.Gateway;
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
        private string _joinedAt;
        public DateTime? JoinedAt
        {
            get
            {
                if (_joinedAt == null)
                    return null;

                return DiscordTimestamp.FromString(_joinedAt);
            }
        }

        [JsonProperty("premium_since")]
        private string _premiumSince;
        public DateTime? BoostingSince
        {
            get
            {
                if (_premiumSince == null)
                    return null;
                else
                    return DiscordTimestamp.FromString(_premiumSince);
            }
        }


        [JsonProperty("mute")]
        public bool Muted { get; protected set; }


        [JsonProperty("deaf")]
        public bool Deafened { get; protected set; }


        public new void Dispose()
        {
            base.Dispose();
            Nickname = null;
            _roles = null;
            _joinedAt = null;
            _premiumSince = null;
        }
    }
}

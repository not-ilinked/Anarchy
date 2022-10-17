using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anarchy;
using Newtonsoft.Json;

namespace Discord
{
    public class DiscordGuild : InviteGuild, IDisposable
    {
        public DiscordGuild()
        {
            OnClientUpdated += (sender, e) =>
            {
                if (!Unavailable)
                {
                    lock (_roles.Lock)
                    {
                        _roles.SetClientsInList(Client);

                        foreach (var role in _roles)
                            role.GuildId = Id;
                    }

                    Emojis.SetClientsInList(Client);
                }
            };
        }

        [JsonProperty("discovery_splash")]
        private string _discoverySplashHash;

        public DiscordCDNImage DiscoverySplash
        {
            get
            {
                if (_discoverySplashHash == null)
                    return null;
                else
                    return new DiscordCDNImage(CDNEndpoints.DiscoverySplash, Id, _discoverySplashHash);
            }
        }

        [JsonProperty("max_members")]
        public uint MaxMembers { get; private set; }

        [JsonProperty("max_video_channel_users")]
        public uint MaxLivestreams { get; private set; }

        [JsonProperty("preferred_locale")]
        public DiscordLanguage? PreferredLanguage { get; private set; }

        [JsonProperty("rules_channel_id")]
        private ulong? _rulesChannelId;

        public MinimalTextChannel RulesChannel
        {
            get
            {
                if (_rulesChannelId.HasValue)
                    return new MinimalTextChannel(_rulesChannelId.Value).SetClient(Client);
                else
                    return null;
            }
        }

        [JsonProperty("public_updates_channel_id")]
        private ulong? _updateChannelId;

        public MinimalTextChannel PublicUpdatesChannel
        {
            get
            {
                if (_updateChannelId.HasValue)
                    return new MinimalTextChannel(_updateChannelId.Value).SetClient(Client);
                else
                    return null;
            }
        }

        [JsonProperty("unavailable")]
        public bool Unavailable { get; internal set; }

        [JsonProperty("premium_subscription_count")]
        public uint? NitroBoosts { get; private set; }

        [JsonProperty("region")]
        public string Region { get; private set; }

        [JsonProperty("default_message_notifications")]
        public GuildDefaultNotifications DefaultNotifications { get; private set; }

        [JsonProperty("premium_tier")]
        public GuildPremiumTier PremiumTier { get; private set; }

        [JsonProperty("roles")]
        internal ConcurrentList<DiscordRole> _roles;

        public IReadOnlyList<DiscordRole> Roles
        {
            get { return _roles; }
        }

        public DiscordRole EveryoneRole
        {
            get
            {
                return Unavailable ? null : Roles.First(r => r.Name == "@everyone");
            }
        }

        [JsonProperty("emojis")]
        internal List<DiscordEmoji> _emojis;

        public IReadOnlyList<DiscordEmoji> Emojis
        {
            get
            {
                if (!Unavailable)
                {
                    foreach (var emoji in _emojis)
                        emoji.GuildId = Id;
                }

                return _emojis;
            }
        }

        [JsonProperty("owner_id")]
        public ulong OwnerId { get; private set; }

        [JsonProperty("mfa_level")]
        public bool MfaRequired { get; private set; }

        [JsonProperty("system_channel_id")]
        private ulong? _sysChannelId;

        [JsonProperty("system_channel_flags")]
        private int _sysChannelFlags;

        public SystemChannelInformation SystemChannel
        {
            get { return new SystemChannelInformation(_sysChannelId, _sysChannelFlags).SetClient(Client); }
        }

        internal void Update(DiscordGuild guild)
        {
            base.Update(guild);

            Unavailable = guild.Unavailable;
            _discoverySplashHash = guild._discoverySplashHash;
            Region = guild.Region;
            _roles = guild._roles;
            _emojis = guild._emojis;
            DefaultNotifications = guild.DefaultNotifications;
            PremiumTier = guild.PremiumTier;
            OwnerId = guild.OwnerId;
            MfaRequired = guild.MfaRequired;
            NitroBoosts = guild.NitroBoosts;
            _rulesChannelId = guild._rulesChannelId;
            _updateChannelId = guild._updateChannelId;
            _sysChannelId = guild._sysChannelId;
            _sysChannelFlags = guild._sysChannelFlags;
        }

        public async Task UpdateAsync()
        {
            Update(await Client.GetGuildAsync(Id));
        }

        /// <summary>
        /// Updates the guild's info
        /// </summary>
        public void Update()
        {
            UpdateAsync().GetAwaiter().GetResult();
        }

        public new async Task ModifyAsync(GuildProperties properties)
        {
            Update(await Client.ModifyGuildAsync(Id, properties));
        }

        /// <summary>
        /// Modifies the guild
        /// </summary>
        /// <param name="properties">Options for modifying the guild</param>
        public new void Modify(GuildProperties properties)
        {
            ModifyAsync(properties).GetAwaiter().GetResult();
        }

        public override async Task<IReadOnlyList<DiscordRole>> GetRolesAsync()
        {
            var roles = await base.GetRolesAsync();
            _roles = new ConcurrentList<DiscordRole>(roles);

            return roles;
        }

        /// <summary>
        /// Gets the guild's roles
        /// </summary>
        public override IReadOnlyList<DiscordRole> GetRoles()
        {
            return GetRolesAsync().Result;
        }

        public override async Task<IReadOnlyList<DiscordRole>> SetRolePositionsAsync(List<RolePositionUpdate> positions)
        {
            var roles = await base.SetRolePositionsAsync(positions);
            _roles = new ConcurrentList<DiscordRole>(roles);

            return roles;
        }

        public override IReadOnlyList<DiscordRole> SetRolePositions(List<RolePositionUpdate> roles)
        {
            return SetRolePositionsAsync(roles).GetAwaiter().GetResult();
        }

        public override async Task<IReadOnlyList<DiscordEmoji>> GetEmojisAsync()
        {
            return _emojis = (await base.GetEmojisAsync()).ToList();
        }

        /// <summary>
        /// Gets the guild's emojis
        /// </summary>
        public override IReadOnlyList<DiscordEmoji> GetEmojis()
        {
            return GetEmojisAsync().Result;
        }

        public new void Dispose()
        {
            base.Dispose();
            Description = null;
            Region = null;
            _roles = null;
            _emojis = null;
        }
    }
}
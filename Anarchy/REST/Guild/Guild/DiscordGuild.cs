using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Discord
{
    public class DiscordGuild : BaseGuild, IDisposable
    {
        public DiscordGuild()
        {
            OnClientUpdated += (sender, e) =>
            {
                Roles.SetClientsInList(Client);
                Emojis.SetClientsInList(Client);
            };
        }


        [JsonProperty("description")]
        public string Description { get; protected set; }


        [JsonProperty("unavailable")]
        public bool Unavailable { get; internal set; }


        [JsonProperty("premium_subscription_count")]
        public uint? NitroBoosts { get; private set; }


        [JsonProperty("region")]
        public string Region { get; private set; }


        [JsonProperty("vanity_url_code")]
        public string VanityInvite { get; private set; }


        [JsonProperty("verification_level")]
        public GuildVerificationLevel VerificationLevel { get; private set; }


        [JsonProperty("default_message_notifications")]
        public GuildDefaultNotifications DefaultNotifications { get; private set; }


        [JsonProperty("premium_tier")]
        public GuildPremiumTier PremiumTier { get; private set; }


        [JsonProperty("features")]
        public IReadOnlyList<string> Features { get; private set; }


        [JsonProperty("roles")]
        internal List<DiscordRole> _roles;
        
        public IReadOnlyList<DiscordRole> Roles
        {
            get
            {
                if (!Unavailable)
                {
                    foreach (var role in _roles)
                        role.GuildId = Id;
                }
                
                return _roles;
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
        private readonly ulong? _sysChannelId; 


        [JsonProperty("system_channel_flags")]
        private readonly int _sysChannelFlags;


        public SystemChannelInformation SystemChannel
        {
            get { return new SystemChannelInformation(_sysChannelId, _sysChannelFlags).SetClient(Client); }
        }


        internal void Update(DiscordGuild guild)
        {
            Name = guild.Name;
            _iconId = guild.Icon.Hash;
            Region = guild.Region;
            _roles = guild.Roles.ToList();
            _emojis = guild.Emojis.ToList();
            VerificationLevel = guild.VerificationLevel;
            DefaultNotifications = guild.DefaultNotifications;
            OwnerId = guild.OwnerId;
            VanityInvite = guild.VanityInvite;
        }


        /// <summary>
        /// Updates the guild's info
        /// </summary>
        public void Update()
        {
            Update(Client.GetGuild(Id));
        }


        /// <summary>
        /// Modifies the guild
        /// </summary>
        /// <param name="properties">Options for modifying the guild</param>
        public void Modify(GuildProperties properties)
        {
            Update(Client.ModifyGuild(Id, properties));
        }


        /// <summary>
        /// Gets the guild's roles
        /// </summary>
        public override IReadOnlyList<DiscordRole> GetRoles()
        {
            return _roles = base.GetRoles().ToList();
        }


        /// <summary>
        /// Gets the guild's emojis
        /// </summary>
        public override IReadOnlyList<DiscordEmoji> GetEmojis()
        {
            return _emojis = base.GetEmojis().ToList();
        }


        public new void Dispose()
        {
            base.Dispose();
            Description = null;
            Region = null;
            VanityInvite = null;
            Features = null;
            _roles = null;
            _emojis = null;
        }
    }
}
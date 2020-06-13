using Discord.Gateway;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Discord
{
    public class GuildMember : PartialGuildMember, IDisposable
    {
        public GuildMember()
        {
            OnClientUpdated += (sender, e) => User.SetClient(Client);
        }


        [JsonProperty("user")]
        public DiscordUser User { get; internal set; }


        /// <summary>
        /// Updates the member's information
        /// </summary>
        public void Update()
        {
            GuildMember member = Client.GetGuildMember(GuildId, User.Id);
            User = member.User;
            Nickname = member.Nickname;
            _roles = member._roles;
            Muted = member.Muted;
            Deafened = member.Deafened;
        }


        /// <summary>
        /// Modifies the specified guild member
        /// </summary>
        /// <param name="properties">Things to change</param>
        public void Modify(GuildMemberProperties properties)
        {
            Client.ModifyGuildMember(GuildId, User.Id, properties);
        }

        /// <summary>
        /// Mutes the user in the specified guild
        /// </summary>
        /// <param name="unmute">Unmute the user instead of muting them</param>
        public void Mute(bool unmute = false)
        {
            Client.ModifyGuildMember(GuildId, User.Id, new GuildMemberProperties() { Muted = !unmute });
        }


        /// <summary>
        /// Deafenes the user in the specified guild
        /// </summary>
        /// <param name="guildId">ID of the guild</param>
        /// <param name="userId">ID of the user</param>
        /// <param name="undeafen">Undeafen the user instead of deafening them</param>
        public void Deafen(bool undeafen = false)
        {
            Client.ModifyGuildMember(GuildId, User.Id, new GuildMemberProperties() { Deafened = !undeafen });
        }


        /// <summary>
        /// Sets the member's roles
        /// </summary>
        public void SetRoles(List<ulong> roles)
        {
            Client.SetGuildMemberRoles(GuildId, User.Id, roles);
        }


        /// <summary>
        /// Sets the member's roles
        /// </summary>
        public void SetRoles(List<DiscordRole> roles)
        {
            List<ulong> ids = new List<ulong>();
            foreach (var role in roles)
                ids.Add(role.Id);

            SetRoles(ids);
        }


        /// <summary>
        /// Adds a role to the guild member
        /// </summary>
        /// <param name="roleId">ID of the role</param>
        public void AddRole(ulong roleId)
        {
            Client.AddRoleToUser(GuildId, roleId, User.Id);
        }


        /// <summary>
        /// Adds a role to the guild member
        /// </summary>
        public void AddRole(DiscordRole role)
        {
            AddRole(role.Id);
        }


        /// <summary>
        /// Removes a role from the guild member
        /// </summary>
        /// <param name="roleId">ID of the role</param>
        public void RemoveRole(ulong roleId)
        {
            Client.RemoveRoleFromUser(GuildId, roleId, User.Id);
        }


        /// <summary>
        /// Removes a role from the guild member
        /// </summary>
        public void RemoveRole(DiscordRole role)
        {
            RemoveRole(role.Id);
        }


        public bool HasPermission(DiscordPermission permission)
        {
            DiscordGuild guild = null;

            if (SocketClient)
            {
                var socketClient = (DiscordSocketClient)Client;

                if (socketClient.Config.Cache)
                    guild = socketClient.GetCachedGuild(GuildId);
            }

            if (guild == null)
                guild = Client.GetGuild(GuildId);

            if (guild.OwnerId == User.Id)
                return true;

            return guild.Roles.Where(r => _roles.Contains(r.Id) && (r.Permissions.Has(permission) || r.Permissions.Has(DiscordPermission.Administrator))).Count() > 0;
        }



        /// <summary>
        /// Kicks the member from the guild
        /// </summary>
        public void Kick()
        {
            Client.KickGuildMember(GuildId, User.Id);
        }


        /// <summary>
        /// Bans the member from the guild
        /// </summary>
        /// <param name="reason">The reason for banning the user</param>
        /// <param name="deleteMessageDays">Amount of days to purge messages (max is 7)</param>
        public void Ban(string reason = null, uint deleteMessageDays = 0)
        {
            Client.BanGuildMember(GuildId, User.Id, reason, deleteMessageDays);
        }


        /// <summary>
        /// Unbans the user from the guild
        /// </summary>
        public void Unban()
        {
            Client.UnbanGuildMember(GuildId, User.Id);
        }


        public string AsMessagable()
        {
            return User.AsMessagable();
        }


        public override string ToString()
        {
            return User.ToString();
        }


        public static implicit operator ulong(GuildMember instance)
        {
            return instance.User.Id;
        }


        public new void Dispose()
        {
            base.Dispose();
            User = null;
        }
    }
}
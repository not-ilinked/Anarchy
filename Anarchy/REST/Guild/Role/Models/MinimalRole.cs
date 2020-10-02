using Newtonsoft.Json;

namespace Discord
{
    public class MinimalRole : Controllable
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }

        internal ulong GuildId { get; set; }

        public MinimalGuild Guild
        {
            get { return new MinimalGuild(GuildId); }
        }

        public MinimalRole()
        { }


        public MinimalRole(ulong guildId, ulong roleId)
        {
            GuildId = guildId;
            Id = roleId;
        }


        /// <summary>
        /// Modifies the role
        /// </summary>
        /// <param name="properties">Options for modifying the role</param>
        public DiscordRole Modify(RoleProperties properties)
        {
            return Client.ModifyRole(GuildId, Id, properties);
        }


        /// <summary>
        /// Deletes the role
        /// </summary>
        public void Delete()
        {
            Client.DeleteRole(GuildId, Id);
        }


        public string AsMessagable()
        {
            return $"<@&{Id}>";
        }
    }
}

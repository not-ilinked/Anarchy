using System.Drawing;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Discord.Commands;


namespace Discord
{
    public class DiscordRole : Controllable, IMentionable
    {
        [JsonPropertyName("id")]
        public ulong Id { get; private set; }

        internal ulong GuildId { get; set; }

        public MinimalGuild Guild
        {
            get { return new MinimalGuild(GuildId).SetClient(Client); }
        }

        [JsonPropertyName("name")]
        public string Name { get; private set; }

        [JsonPropertyName("color")]
        private uint _color;
        public Color Color
        {
            get { return Color.FromArgb((int) _color); }
            private set { _color = (uint) Color.FromArgb(0, value.R, value.G, value.B).ToArgb(); }
        }

        [JsonPropertyName("position")]
        public int Position { get; private set; }

        [JsonPropertyName("hoist")]
        public bool Seperated { get; private set; }

        [JsonPropertyName("mentionable")]
        public bool Mentionable { get; private set; }

        [JsonPropertyName("permissions")]
        public DiscordPermission Permissions { get; private set; }

        public async Task<DiscordRole> ModifyAsync(RoleProperties properties)
        {
            return await Client.ModifyRoleAsync(GuildId, Id, properties);
        }

        /// <summary>
        /// Modifies the role
        /// </summary>
        /// <param name="properties">Options for modifying the role</param>
        public DiscordRole Modify(RoleProperties properties)
        {
            return ModifyAsync(properties).GetAwaiter().GetResult();
        }

        public async Task DeleteAsync()
        {
            await Client.DeleteRoleAsync(GuildId, Id);
        }

        /// <summary>
        /// Deletes the role
        /// </summary>
        public void Delete()
        {
            DeleteAsync().GetAwaiter().GetResult();
        }

        public string AsMessagable()
        {
            return $"<@&{Id}>";
        }

        public override string ToString()
        {
            return Name;
        }

        public static implicit operator ulong(DiscordRole instance)
        {
            return instance.Id;
        }
    }
}
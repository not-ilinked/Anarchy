using Newtonsoft.Json;
using System.Drawing;

namespace Discord
{
    public class DiscordRole : MinimalRole
    {
        [JsonProperty("name")]
        public string Name { get; private set; }


        [JsonProperty("color")]
        private uint _color;
        public Color Color
        {
            get { return Color.FromArgb((int)_color); }
            private set { _color = (uint)Color.FromArgb(0, value.R, value.G, value.B).ToArgb(); }
        }


        [JsonProperty("position")]
        public int Position { get; private set; }


        [JsonProperty("hoist")]
        public bool Seperated { get; private set; }


        [JsonProperty("mentionable")]
        public bool Mentionable { get; private set; }


        [JsonProperty("permissions")]
        public DiscordPermission Permissions { get; private set; }


        /// <summary>
        /// Modifies the role
        /// </summary>
        /// <param name="properties">Options for modifying the role</param>
        public new void Modify(RoleProperties properties)
        {
            DiscordRole role = base.Modify(properties);
            Name = role.Name;
            Permissions = role.Permissions;
            Color = role.Color;
            Seperated = role.Seperated;
            Position = role.Position;
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
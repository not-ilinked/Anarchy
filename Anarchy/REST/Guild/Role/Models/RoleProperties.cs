using Newtonsoft.Json;
using System.Drawing;

namespace Discord
{
    /// <summary>
    /// Options for creating/modifying a role
    /// </summary>
    public class RoleProperties
    {
        private readonly Property<string> NameProperty = new Property<string>();
        [JsonProperty("name")]
        public string Name
        {
            get { return NameProperty; }
            set { NameProperty.Value = value; }
        }


        public bool ShouldSerializeName()
        {
            return NameProperty.Set;
        }


        private readonly Property<DiscordEditablePermissions> PermissionsProperty = new Property<DiscordEditablePermissions>();
        [JsonProperty("permissions")]
#pragma warning disable IDE1006, IDE0051
        private uint _permissions
        {
            get { return Permissions; }
        }
#pragma warning restore IDE1006, IDE0051
        public DiscordEditablePermissions Permissions
        {
            get { return PermissionsProperty; }
            set { PermissionsProperty.Value = value; }
        }


        public bool ShouldSerialize_permissions()
        {
            return PermissionsProperty.Set;
        }


        private readonly Property<uint> ColorProperty = new Property<uint>();
        [JsonProperty("color")]
#pragma warning disable IDE1006, IDE0051
        private uint _color
        {
            get { return ColorProperty; }
            set { ColorProperty.Value = value; }
        }
#pragma warning restore IDE1006, IDE0051
        public Color Color
        {
            get { return Color.FromArgb((int)_color); }
            set { _color = (uint)Color.FromArgb(0, value.R, value.G, value.B).ToArgb(); }
        }


        public bool ShouldSerialize_color()
        {
            return ColorProperty.Set;
        }


        private readonly Property<bool> SeperatedProperty = new Property<bool>();
        [JsonProperty("hoist")]
        public bool Seperated
        {
            get { return SeperatedProperty; }
            set { SeperatedProperty.Value = value; }
        }


        public bool ShouldSerializeSeperated()
        {
            return SeperatedProperty.Set;
        }


        private readonly Property<bool> MentionableProperty = new Property<bool>();
        [JsonProperty("mentionable")]
        public bool Mentionable
        {
            get { return MentionableProperty; }
            set { MentionableProperty.Value = value; }
        }


        public bool ShouldSerializeMentionable()
        {
            return MentionableProperty.Set;
        }


        public override string ToString()
        {
            return Name;
        }
    }
}
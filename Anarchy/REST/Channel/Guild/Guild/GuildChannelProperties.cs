using System.Text.Json.Serialization;

namespace Discord
{
    /// <summary>
    /// Options for modifying a <see cref="GuildChannel"/>
    /// </summary>
    public class GuildChannelProperties
    {
        private readonly DiscordParameter<string> NameProperty = new DiscordParameter<string>();
        [JsonPropertyName("name")]
        public string Name
        {
            get { return NameProperty; }
            set { NameProperty.Value = value; }
        }

        public bool ShouldSerializeName()
        {
            return NameProperty.Set;
        }

        private readonly DiscordParameter<ulong?> ParentProperty = new DiscordParameter<ulong?>();
        [JsonPropertyName("parent_id")]
        public ulong? ParentId
        {
            get { return ParentProperty; }
            set { ParentProperty.Value = value; }
        }

        public bool ShouldSerializeParentId()
        {
            return ParentProperty.Set;
        }

        private readonly DiscordParameter<uint> PositionProperty = new DiscordParameter<uint>();
        [JsonPropertyName("position")]
        public uint Position
        {
            get { return PositionProperty; }
            set { PositionProperty.Value = value; }
        }

        public bool ShouldSerializePosition()
        {
            return PositionProperty.Set;
        }
    }
}

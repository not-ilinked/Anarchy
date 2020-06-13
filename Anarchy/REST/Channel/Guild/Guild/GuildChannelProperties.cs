using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Options for modifying a <see cref="GuildChannel"/>
    /// </summary>
    public class GuildChannelProperties : ChannelProperties
    {
        private readonly Property<ulong?> ParentProperty = new Property<ulong?>();
        [JsonProperty("parent_id")]
        public ulong? ParentId
        {
            get { return ParentProperty; }
            set { ParentProperty.Value = value; }
        }


        public bool ShouldSerializeParentId()
        {
            return ParentProperty.Set;
        }


        private readonly Property<uint> PositionProperty = new Property<uint>();
        [JsonProperty("position")]
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

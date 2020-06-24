using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Options for modifying a <see cref="GuildChannel"/>
    /// </summary>
    public class GuildChannelProperties : ChannelProperties
    {
        private readonly DiscordParameter<ulong?> ParentProperty = new DiscordParameter<ulong?>();
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


        private readonly DiscordParameter<uint> PositionProperty = new DiscordParameter<uint>();
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

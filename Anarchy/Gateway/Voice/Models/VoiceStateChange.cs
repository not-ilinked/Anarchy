using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class VoiceStateChange
    {
        [JsonProperty("guild_id")]
        public ulong? GuildId { get; set; }


        [JsonProperty("channel_id")]
        public ulong? ChannelId { get; set; }


        private Property<bool> _mutedProperty = new Property<bool>();
        [JsonProperty("self_mute")]
        public bool Muted
        {
            get { return _mutedProperty; }
            set { _mutedProperty.Value = value; }
        }


        public bool ShouldSerializeMuted()
        {
            return _mutedProperty.Set;
        }


        private Property<bool> _deafProperty = new Property<bool>();
        [JsonProperty("self_deaf")]
        public bool Deafened
        {
            get { return _deafProperty; }
            set { _deafProperty.Value = value; }
        }


        public bool ShouldSerializeDeafened()
        {
            return _deafProperty.Set;
        }
    }
}

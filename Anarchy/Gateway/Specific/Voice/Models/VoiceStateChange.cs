using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class VoiceStateChange
    {
        [JsonProperty("guild_id")]
        public ulong? GuildId { get; set; }


        internal readonly DiscordParameter<ulong?> ChannelProperty = new DiscordParameter<ulong?>();
        [JsonProperty("channel_id")]
        public ulong? ChannelId 
        {
            get { return ChannelProperty; }
            set { ChannelProperty.Value = value; }
        }


        internal readonly DiscordParameter<bool> MutedProperty = new DiscordParameter<bool>();
        [JsonProperty("self_mute")]
        public bool Muted
        {
            get { return MutedProperty; }
            set { MutedProperty.Value = value; }
        }


        internal readonly DiscordParameter<bool> DeafProperty = new DiscordParameter<bool>();
        [JsonProperty("self_deaf")]
        public bool Deafened
        {
            get { return DeafProperty; }
            set { DeafProperty.Value = value; }
        }


        internal readonly DiscordParameter<bool> VideoProperty = new DiscordParameter<bool>();
        [JsonProperty("self_video")]
        internal bool Screensharing
        {
            get { return VideoProperty; }
            set { VideoProperty.Value = value; }
        }
    }
}

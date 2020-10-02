using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class VoiceStateProperties
    {
        internal readonly DiscordParameter<ulong?> GuildProperty = new DiscordParameter<ulong?>();
        [JsonProperty("guild_id")]
        public ulong? GuildId
        {
            get { return GuildProperty; }
            set { GuildProperty.Value = value; }
        }


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
        public bool Video
        {
            get { return VideoProperty; }
            set { VideoProperty.Value = value; }
        }
    }
}

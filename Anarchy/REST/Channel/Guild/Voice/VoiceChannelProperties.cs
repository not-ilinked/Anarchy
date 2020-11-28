using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Options for modifying a <see cref="VoiceChannel"/>
    /// </summary>
    public class VoiceChannelProperties : GuildChannelProperties
    {
        private readonly DiscordParameter<uint> BitrateProperty = new DiscordParameter<uint>();
        [JsonProperty("bitrate")]
        public uint Bitrate
        {
            get { return BitrateProperty; }
            set { BitrateProperty.Value = value; }
        }


        public bool ShouldSerializeBitrate()
        {
            return BitrateProperty.Set;
        }


        private readonly DiscordParameter<uint> UserLimitProperty = new DiscordParameter<uint>();
        [JsonProperty("user_limit")]
        public uint UserLimit
        {
            get { return UserLimitProperty; }
            set { UserLimitProperty.Value = value; }
        }


        public bool ShouldSerializeUserLimit()
        {
            return UserLimitProperty.Set;
        }
    }
}

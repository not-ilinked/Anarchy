using Newtonsoft.Json;

namespace Discord
{
    public class UserReportIdentification
    {
        [JsonProperty("reason")]
        internal DiscordReportReason Reason { get; set; }

        private DiscordParameter<ulong> _userParam = new DiscordParameter<ulong>();
        [JsonProperty("user_id")]
        public ulong UserId
        {
            get { return _userParam; }
            set { _userParam.Value = value; }
        }

        private readonly DiscordParameter<ulong> _guildParam = new DiscordParameter<ulong>();
        [JsonProperty("guild_id")]
        public ulong GuildId
        {
            get { return _guildParam; }
            set { _guildParam.Value = value; }
        }

        public bool ShouldSerializeGuildId() => _guildParam.Set;

        private readonly DiscordParameter<ulong> _channelParam = new DiscordParameter<ulong>();
        [JsonProperty("channel_id")]
        public ulong ChannelId
        {
            get { return _channelParam; }
            set { _channelParam.Value = value; }
        }

        public bool ShouldSerializeChannelId() => _channelParam.Set;

        private readonly DiscordParameter<ulong> _messageParam = new DiscordParameter<ulong>();
        [JsonProperty("message_id")]
        public ulong MessageId
        {
            get { return _messageParam; }
            set { _messageParam.Value = value; }
        }

        public bool ShouldSerializeMessageId() => _messageParam.Set;
    }
}

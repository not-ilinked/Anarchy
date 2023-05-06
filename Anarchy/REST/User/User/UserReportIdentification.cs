

using System.Text.Json.Serialization;

namespace Discord
{
    public class UserReportIdentification
    {
        [JsonPropertyName("reason")]
        internal DiscordReportReason Reason { get; set; }

        private readonly DiscordParameter<ulong> _userParam = new();
        [JsonPropertyName("user_id")]
        public ulong UserId
        {
            get { return _userParam; }
            set { _userParam.Value = value; }
        }

        private readonly DiscordParameter<ulong> _guildParam = new();
        [JsonPropertyName("guild_id")]
        public ulong GuildId
        {
            get { return _guildParam; }
            set { _guildParam.Value = value; }
        }

        public bool ShouldSerializeGuildId() => _guildParam.Set;

        private readonly DiscordParameter<ulong> _channelParam = new();
        [JsonPropertyName("channel_id")]
        public ulong ChannelId
        {
            get { return _channelParam; }
            set { _channelParam.Value = value; }
        }

        public bool ShouldSerializeChannelId() => _channelParam.Set;

        private readonly DiscordParameter<ulong> _messageParam = new();
        [JsonPropertyName("message_id")]
        public ulong MessageId
        {
            get { return _messageParam; }
            set { _messageParam.Value = value; }
        }

        public bool ShouldSerializeMessageId() => _messageParam.Set;
    }
}

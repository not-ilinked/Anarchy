

using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class DiscordVoiceState : Controllable
    {
        public DiscordVoiceState()
        {
            OnClientUpdated += (sender, e) =>
            {
                Member.SetClient(Client);
                Channel.SetClient(Client);
                Guild.SetClient(Client);
            };
        }

        [JsonPropertyName("channel_id")]
        internal ulong? ChannelId { get; set; }

        public MinimalChannel Channel
        {
            get
            {
                if (ChannelId.HasValue)
                    return new MinimalChannel(ChannelId.Value).SetClient(Client);
                else
                    return null;
            }
        }

        [JsonPropertyName("member")]
        private readonly GuildMember _member;

        [JsonIgnore]
        public GuildMember Member
        {
            get
            {
                if (_member != null)
                    _member.GuildId = Guild;

                return _member;
            }
        }

        [JsonPropertyName("user_id")]
        public ulong UserId { get; private set; }

        [JsonPropertyName("guild_id")]
        private ulong? _guildId;

        public MinimalGuild Guild
        {
            get
            {
                if (_guildId.HasValue)
                    return new MinimalGuild(_guildId.Value).SetClient(Client);
                else
                    return null;
            }
            internal set
            {
                _guildId = value.Id;
            }
        }

        [JsonPropertyName("mute")]
        public bool Muted { get; private set; }

        [JsonPropertyName("deaf")]
        public bool Deafened { get; private set; }

        [JsonPropertyName("self_deaf")]
        public bool SelfDeafened { get; private set; }

        [JsonPropertyName("self_mute")]
        public bool SelfMuted { get; private set; }

        [JsonPropertyName("self_video")]
        public bool Video { get; private set; }

        [JsonPropertyName("self_stream")]
        public bool Streaming { get; private set; }

        [JsonPropertyName("session_id")]
        internal string SessionId { get; private set; }
    }
}

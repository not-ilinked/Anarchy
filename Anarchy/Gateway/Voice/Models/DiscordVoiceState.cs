using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class DiscordVoiceState : Controllable
    {
        public DiscordVoiceState()
        {
            OnClientUpdated += (sender, e) => Member.SetClient(Client);
        }


        [JsonProperty("channel_id")]
        private readonly ulong? _channelId;

        public MinimalChannel Channel
        {
            get
            {
                if (_channelId.HasValue)
                    return new MinimalChannel(_channelId.Value).SetClient(Client);
                else
                    return null;
            }
        }


        [JsonProperty("member")]
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


        [JsonProperty("user_id")]
        public ulong UserId { get; private set; }


        [JsonProperty("guild_id")]
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


        [JsonProperty("mute")]
        public bool Muted { get; private set; }


        [JsonProperty("deaf")]
        public bool Deafened { get; private set; }


        [JsonProperty("self_deaf")]
        public bool SelfDeafened { get; private set; }


        [JsonProperty("self_mute")]
        public bool SelfMuted { get; private set; }


        [JsonProperty("self_stream")]
        public bool Streaming { get; private set; }
    }
}

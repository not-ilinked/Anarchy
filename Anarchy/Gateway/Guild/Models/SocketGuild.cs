using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Discord.Gateway
{
    public class SocketGuild : DiscordGuild, IDisposable
    {
        public SocketGuild()
        {
            OnClientUpdated += (sender, e) => Channels.SetClientsInList(Client);
            JsonUpdated += (sender, json) =>
            {
                if (!Unavailable)
                    _channels = json.Value<JArray>("channels").PopulateListJson<GuildChannel>();
            };
        }


        [JsonProperty("large")]
        public bool Large { get; private set; }


        [JsonProperty("member_count")]
        public uint MemberCount { get; private set; }


        internal List<GuildChannel> _channels;
        [JsonIgnore]
        public IReadOnlyList<GuildChannel> Channels
        {
            get
            {
                if (!Unavailable)
                {
                    foreach (var channel in _channels)
                    {
                        channel.GuildId = Id;
                        channel.Json["guild_id"] = Id;
                    }
                }

                return _channels;
            }
        }


        [JsonProperty("joined_at")]
        public DateTime JoinedAt { get; private set; }


        [JsonProperty("voice_states")]
        internal List<DiscordVoiceState> _voiceStates;

        public IReadOnlyList<DiscordVoiceState> VoiceStates
        {
            get
            {
                foreach (var state in _voiceStates)
                    state.Guild = this;

                return _voiceStates;
            }
        }


        public IReadOnlyList<GuildMember> GetMembers(uint limit = 0)
        {
            return ((DiscordSocketClient)Client).GetGuildMembers(Id, limit);
        }


        public IReadOnlyList<GuildMember> GetChannelMembers(ulong channelId, MemberListQueryOptions options = null)
        {
            return ((DiscordSocketClient)Client).GetGuildChannelMembers(Id, channelId, options);
        }


        /// <summary>
        /// Gets the guild's channels
        /// </summary>
        public override IReadOnlyList<GuildChannel> GetChannels()
        {
            var channels = base.GetChannels();
            _channels = channels.ToList();
            return channels;
        }


        public new void Dispose()
        {
            base.Dispose();
            _channels = null;
            _voiceStates = null;
        }
    }
}

using Anarchy;
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
            OnClientUpdated += (sender, e) => 
            {
                if (!Unavailable)
                {
                    Members.SetClientsInList(Client);

                    foreach (var member in Members)
                        member.GuildId = Id;

                    lock (ChannelsConcurrent.Lock)
                    {
                        ChannelsConcurrent.SetClientsInList(Client);

                        foreach (var channel in ChannelsConcurrent)
                            channel.GuildId = Id;
                    }

                    lock (_voiceStates.Lock)
                    {
                        _voiceStates.SetClientsInList(Client);

                        foreach (var state in _voiceStates)
                            state.Guild = this;
                    }
                }
            };
        }


        [JsonProperty("large")]
        public bool Large { get; private set; }


        [JsonProperty("member_count")]
        public uint MemberCount { get; internal set; }


        // i'm not 100% sure of how this functions yet. All i have so far is https://discord.com/developers/docs/topics/gateway#request-guild-members, but it doesn't seem like that applies to users.
        [JsonProperty("members")]
        internal List<GuildMember> Members { get; private set; }

        private GuildMember _member;
        public GuildMember ClientMember
        {
            get
            {
                if (!Unavailable && _member == null)
                    _member = Members.First(m => m.User.Id == Client.User.Id);

                return _member;
            }
            internal set { _member = value; }
        }


        [JsonProperty("channels")]
        [JsonConverter(typeof(DeepJsonConverter<GuildChannel>))]
        internal ConcurrentList<GuildChannel> ChannelsConcurrent;

        public IReadOnlyList<GuildChannel> Channels
        {
            get
            {
                if (!Unavailable)
                    return ChannelsConcurrent;
                else
                    return new List<GuildChannel>();
            }
        }


        [JsonProperty("joined_at")]
        public DateTime CreatedAt { get; private set; }


        [JsonProperty("presences")]
        public IReadOnlyList<DiscordPresence> Presences { get; private set; }


        [JsonProperty("voice_states")]
        internal ConcurrentList<DiscordVoiceState> _voiceStates;

        public IReadOnlyList<DiscordVoiceState> VoiceStates
        {
            get
            {
                if (!Unavailable)
                    lock (_voiceStates.Lock) { return _voiceStates; }
                else
                    return new List<DiscordVoiceState>();
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


        public new void Dispose()
        {
            base.Dispose();

            ChannelsConcurrent.Clear();
            _voiceStates.Clear();
        }
    }
}

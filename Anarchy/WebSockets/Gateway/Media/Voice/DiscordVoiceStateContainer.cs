using Anarchy;
using System.Collections.Generic;

namespace Discord.Gateway
{
    public class DiscordVoiceStateContainer
    {
        public DiscordVoiceStateContainer(ulong userId)
        {
            UserId = userId;
            GuildStates = new ConcurrentDictionary<ulong, DiscordVoiceState>();
        }

        public ulong UserId { get; private set; }
        internal ConcurrentDictionary<ulong, DiscordVoiceState> GuildStates { get; private set; }
        public Dictionary<ulong, DiscordVoiceState> GuildVoiceStates
        {
            get { return new Dictionary<ulong, DiscordVoiceState>(GuildStates); }
        }
        public DiscordVoiceState PrivateChannelVoiceState { get; internal set; }
    }
}

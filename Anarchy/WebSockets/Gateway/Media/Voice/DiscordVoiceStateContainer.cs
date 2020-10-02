using Anarchy;
using System.Collections.Generic;

namespace Discord.Gateway
{
    public class DiscordVoiceStateContainer
    {
        public DiscordVoiceStateContainer(ulong userId)
        {
            UserId = userId;
            GuildVoiceStates = new ConcurrentDictionary<ulong, DiscordVoiceState>();
        }

        public ulong UserId { get; private set; }
        public ConcurrentDictionary<ulong, DiscordVoiceState> GuildVoiceStates { get; private set; }
        public DiscordVoiceState PrivateChannelVoiceState { get; internal set; }
    }
}

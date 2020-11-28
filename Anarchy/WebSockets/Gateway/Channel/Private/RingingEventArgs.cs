using System.Collections.Generic;

namespace Discord.Gateway
{
    public class RingingEventArgs : CallUpdateEventArgs
    {
        public IReadOnlyList<DiscordVoiceState> VoiceStates { get; private set; }

        public RingingEventArgs(DiscordCall call, IReadOnlyList<DiscordVoiceState> states) : base(call)
        {
            VoiceStates = states;
        }
    }
}

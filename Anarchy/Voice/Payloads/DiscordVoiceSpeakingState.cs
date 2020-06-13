using System;

namespace Discord.Voice
{
    [Flags]
    public enum DiscordVoiceSpeakingState
    {
        NotSpeaking = 0,
        Microphone = 1 << 0,
        Soundshare = 1 << 1,
        Priority = 1 << 2
    }
}
